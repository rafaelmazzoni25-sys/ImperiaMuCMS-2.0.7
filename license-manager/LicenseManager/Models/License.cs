using System;
using System.Text.Json.Serialization;

namespace LicenseManager.Models;

public class License
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Key { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public LicenseStatus Status { get; set; } = LicenseStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }
        = DateTime.UtcNow.AddMonths(1);

    public string Notes { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

    public License Clone()
    {
        return new License
        {
            Id = Id,
            Key = Key,
            Email = Email,
            Status = Status,
            CreatedAt = CreatedAt,
            ExpiresAt = ExpiresAt,
            Notes = Notes
        };
    }
}
