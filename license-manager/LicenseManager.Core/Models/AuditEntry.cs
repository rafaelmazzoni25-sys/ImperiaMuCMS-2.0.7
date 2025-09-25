using System;

namespace LicenseManager.Models;

public class AuditEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string Action { get; set; } = string.Empty;

    public string PerformedBy { get; set; } = string.Empty;

    public Guid? LicenseId { get; set; }
        = null;

    public string? LicenseKey { get; set; }
        = null;

    public string? Details { get; set; }
        = null;
}
