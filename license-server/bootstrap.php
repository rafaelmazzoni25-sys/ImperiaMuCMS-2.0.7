<?php
const LICENSE_DATA_FILE = __DIR__ . '/data/licenses.json';
const ENCRYPT_METHOD = 'AES-256-CBC';
const SECRET_KEY = 'XFmva8nIbtoV88dzQoioafgZlipk9dBNhU4nEeS3SHH94LkdES58ThOozVjG0wFdeLPE3ZUhIKMkCPWAn17XzJzQ1Ax3K0zzu2AP2BsxbwLi8HJI73IjkkVAUSphN87Wsxd7cKi8zqSxUIzbe2otwHvVeZH6UhL7yFepgnx0BumReJ2gfAQdAwY8VvS3LBfz5SysoUHlJUuIli7HeuePjtyC6lrfuo1lz6lxKqaCBGecoJNeGoYflkEBJNmkoIF9';
const SECRET_IV = 'xk3xudsF8XjuItROFaMuiDcPHdB0VhCpFx09glr02rO98zcTtT1lmKATtHEeiuKH';

function load_license_data(): array
{
    if (!file_exists(LICENSE_DATA_FILE)) {
        return ['licenses' => []];
    }

    $json = file_get_contents(LICENSE_DATA_FILE);
    if ($json === false || $json === '') {
        return ['licenses' => []];
    }

    $data = json_decode($json, true);
    if (!is_array($data)) {
        return ['licenses' => []];
    }

    if (!isset($data['licenses']) || !is_array($data['licenses'])) {
        $data['licenses'] = [];
    }

    return $data;
}

function save_license_data(array $data): void
{
    if (!isset($data['licenses']) || !is_array($data['licenses'])) {
        $data['licenses'] = [];
    }

    if (!is_dir(dirname(LICENSE_DATA_FILE))) {
        mkdir(dirname(LICENSE_DATA_FILE), 0755, true);
    }

    file_put_contents(LICENSE_DATA_FILE, json_encode($data, JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES));
}

function encrypt_response(array $payload): string
{
    $key = hash('sha256', SECRET_KEY);
    $iv = substr(hash('sha256', SECRET_IV), 0, 16);
    $json = json_encode($payload, JSON_UNESCAPED_SLASHES);
    $encrypted = openssl_encrypt($json, ENCRYPT_METHOD, $key, 0, $iv);
    return base64_encode($encrypted);
}

function respond_encrypted(array $payload): void
{
    header('Content-Type: text/plain; charset=utf-8');
    echo encrypt_response($payload);
    exit;
}

function find_license(array $data, string $key, string $email): ?array
{
    foreach ($data['licenses'] as $index => $license) {
        if (strcasecmp($license['key'] ?? '', $key) === 0 && strcasecmp($license['email'] ?? '', $email) === 0) {
            $license['_index'] = $index;
            return $license;
        }
    }

    return null;
}

function normalize_custom_fields(array $license): array
{
    $defaults = array_fill(0, 21, '');
    $custom = $license['custom_fields'] ?? [];
    foreach ($custom as $position => $value) {
        $defaults[(int) $position] = $value;
    }

    if (!isset($defaults[0])) {
        $defaults[0] = '';
    }

    return $defaults;
}

function ensure_usage_id(array &$license, string $usageId, string $domain): void
{
    if (!isset($license['usages']) || !is_array($license['usages'])) {
        $license['usages'] = [];
    }

    $license['usages'][$usageId] = [
        'domain' => $domain,
        'created_at' => time(),
    ];
}

function build_license_payload(array $license): array
{
    return [
        'response' => 'OKAY',
        'status' => strtoupper($license['status'] ?? 'INACTIVE'),
        'key' => $license['key'] ?? null,
        'identifier' => $license['email'] ?? null,
        'purchase_name' => $license['purchase_name'] ?? null,
        'custom_fields' => normalize_custom_fields($license),
        'expires' => $license['expires'] ?? 0,
    ];
}

function failure_payload(string $status = 'INACTIVE', string $message = 'License not found', int $code = 404): array
{
    return [
        'response' => 'ERROR',
        'status' => strtoupper($status),
        'error_code' => $code,
        'message' => $message,
        'custom_fields' => array_fill(0, 21, ''),
    ];
}

function generate_usage_id(): string
{
    try {
        return bin2hex(random_bytes(8));
    } catch (Throwable $exception) {
        $fallback = openssl_random_pseudo_bytes(8);
        if ($fallback === false) {
            $fallback = pack('N', random_int(0, PHP_INT_MAX)) . pack('N', random_int(0, PHP_INT_MAX));
        }

        return bin2hex($fallback);
    }
}
