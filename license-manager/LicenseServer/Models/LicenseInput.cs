using System;
using LicenseManager.Models;

namespace LicenseServer.Models;

public class LicenseInput
{
    public string Key { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public LicenseType Type { get; set; } = LicenseType.Premium;

    public LicenseStatus Status { get; set; } = LicenseStatus.Active;

    public DateTime? ExpiresAt { get; set; }
        = null;

    public string? Notes { get; set; }
        = string.Empty;
}
