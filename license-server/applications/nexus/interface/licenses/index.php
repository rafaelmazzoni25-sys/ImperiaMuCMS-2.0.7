<?php
require_once __DIR__ . '/../../../../../bootstrap.php';

$data = load_license_data();

$key = $_GET['key'] ?? '';
$identifier = $_GET['identifier'] ?? '';

if ($key === '' || $identifier === '') {
    respond_encrypted(failure_payload('INACTIVE', 'Missing key or identifier', 400));
}

$license = find_license($data, $key, $identifier);
if (!$license) {
    respond_encrypted(failure_payload('INACTIVE', 'License does not exist', 404));
}

$licenseIndex = $license['_index'];
unset($license['_index']);

if (isset($_GET['check'])) {
    $usageId = $_GET['usage_id'] ?? '';
    if ($usageId === '' || !isset($license['usages'][$usageId])) {
        respond_encrypted([
            'response' => 'ERROR',
            'status' => 'INACTIVE',
            'error_code' => 461,
            'message' => 'Usage identifier is not active for this license.',
            'custom_fields' => normalize_custom_fields($license),
        ]);
    }

    $payload = build_license_payload($license);
    $payload['usage_id'] = $usageId;
    respond_encrypted($payload);
}

if (isset($_GET['info'])) {
    $payload = build_license_payload($license);
    respond_encrypted($payload);
}

if (isset($_GET['activate'])) {
    if (strtoupper($license['status'] ?? 'INACTIVE') !== 'ACTIVE') {
        respond_encrypted([
            'response' => 'ERROR',
            'status' => strtoupper($license['status'] ?? 'INACTIVE'),
            'error_code' => 462,
            'message' => 'License is not active and cannot be activated.',
            'custom_fields' => normalize_custom_fields($license),
        ]);
    }

    $extraRaw = $_GET['extra'] ?? '';
    $extra = json_decode($extraRaw, true);
    $domain = '';
    if (is_array($extra) && isset($extra['url'])) {
        $domain = (string) $extra['url'];
    }

    $usageId = null;
    if (isset($license['usages']) && is_array($license['usages'])) {
        foreach ($license['usages'] as $existingId => $meta) {
            if (isset($meta['domain']) && $domain !== '' && strcasecmp($meta['domain'], $domain) === 0) {
                $usageId = $existingId;
                break;
            }
        }
    }

    if ($usageId === null) {
        $usageId = generate_usage_id();
    }

    ensure_usage_id($license, $usageId, $domain);
    $data['licenses'][$licenseIndex] = $license;
    save_license_data($data);

    $payload = build_license_payload($license);
    $payload['response'] = 'OKAY';
    $payload['usage_id'] = $usageId;
    respond_encrypted($payload);
}

respond_encrypted(failure_payload('INACTIVE', 'Unknown request', 400));
