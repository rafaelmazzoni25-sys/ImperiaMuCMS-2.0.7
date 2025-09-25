# ImperiaMuCMS License Server

This folder contains a lightweight, self-hosted replacement for the original ImperiaMuCMS licensing API. The server emulates the handful of HTTP endpoints that the CMS calls during license activation and validation. Host the files on any PHP-capable web server and update the `__IMPERIAMUCMS_LICENSE_SERVER__` constant inside your CMS so it points to the new base URL.

## Provided endpoints

The following routes reproduce the behaviour expected by ImperiaMuCMS 2.0.7:

| Endpoint | Description |
| --- | --- |
| `/apiversion.php` | Returns `1` so the CMS knows that responses are AES encrypted. |
| `/version.php` | Returns the latest application version string (default: `2.0.7`). |
| `/applications/nexus/interface/licenses/?info&key=...&identifier=...` | Returns license metadata such as product name, expiry date and custom fields. |
| `/applications/nexus/interface/licenses/?check&key=...&identifier=...&usage_id=...` | Validates that a previously issued usage identifier is still active. |
| `/applications/nexus/interface/licenses/?activate&key=...&identifier=...&setIdentifier=...&extra={"url":"..."}` | Issues (or reuses) a `usage_id` for the requesting installation. |

All responses are encrypted with the exact same AES-256-CBC key/IV pair that the CMS expects via the `decodeLicData()` helper.

## Managing licenses

Licenses are stored in `data/licenses.json`. Populate the file with your own records:

```
{
  "licenses": [
    {
      "key": "SAMPLE-MAIN-LICENSE",
      "email": "owner@example.com",
      "status": "ACTIVE",
      "purchase_name": "ImperiaMuCMS Premium",
      "product": "premium",
      "expires": 0,
      "custom_fields": {
        "0": "My MU Server",
        "1": "owner@example.com",
        "2": "https://mu.example.com",
        "3": "127.0.0.1",
        "4": "© 2024 My MU Server",
        "5": "no",
        "7": "no",
        "20": "Season 18"
      },
      "usages": {}
    }
  ]
}
```

* `key` and `email` must match the credentials stored inside your CMS license file.
* `status` can be `ACTIVE`, `INACTIVE` or `EXPIRED` (used by the CMS to display the correct error messages).
* `custom_fields` mimics the original Invision Nexus payload. At minimum provide indexes `0` (server name), `2` (domain), `3` (IP) and `20` (season). Any missing index is padded with an empty string automatically.
* `usages` will be filled automatically when the `activate` endpoint is called. Delete an entry to invalidate that installation.

After editing the JSON file you do not need to restart the server—the changes are read on every request.

## Usage flow

1. Copy this folder to your web server root (for example `/var/www/license`).
2. Point your web server vhost to the folder so that `https://licenses.example.com/apiversion.php` is reachable.
3. Update `includes/imperiamucms.php` in the CMS, changing `__IMPERIAMUCMS_LICENSE_SERVER__` to your new domain.
4. Inside the CMS, re-upload or reconfigure your license: the system will contact `/activate`, receive a `usage_id` and store it in `includes/license/license.imperiamucms`.

The server keeps the implementation intentionally simple—extend it as needed to add features like authentication, an admin panel or per-module license keys.

## Testing locally

You can test locally using PHP's built-in server:

```
php -S 0.0.0.0:8000 -t license-server
```

Then point the CMS license URL to `http://127.0.0.1:8000/` while both projects run on the same machine.
