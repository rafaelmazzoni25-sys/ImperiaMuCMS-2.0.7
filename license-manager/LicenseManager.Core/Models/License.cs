using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LicenseManager.Models;

public class License
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Key { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public LicenseType Type { get; set; } = LicenseType.Premium;

    public LicenseStatus Status { get; set; } = LicenseStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }
        = DateTime.UtcNow.AddMonths(1);

    public string Notes { get; set; } = string.Empty;

    public Dictionary<string, LicenseUsage> Usages { get; set; } = new();

    public Dictionary<int, string> CustomFields { get; set; } = new();

    [JsonIgnore]
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

    [JsonIgnore]
    public string PurchaseName => Type switch
    {
        LicenseType.Lite => "ImperiaMuCMS Lite",
        LicenseType.Bronze => "ImperiaMuCMS Bronze",
        LicenseType.Silver => "ImperiaMuCMS Silver",
        LicenseType.Gold => "ImperiaMuCMS Gold",
        LicenseType.Premium => "ImperiaMuCMS Premium",
        LicenseType.PremiumPlus => "ImperiaMuCMS Premium Plus",
        _ => "ImperiaMuCMS"
    };

    [JsonIgnore]
    public string LicenseTypeSlug => Type switch
    {
        LicenseType.Lite => "lite",
        LicenseType.Bronze => "bronze",
        LicenseType.Silver => "silver",
        LicenseType.Gold => "gold",
        LicenseType.Premium => "premium",
        LicenseType.PremiumPlus => "premium+",
        _ => string.Empty
    };

    public License Clone()
    {
        return new License
        {
            Id = Id,
            Key = Key,
            Email = Email,
            Type = Type,
            Status = Status,
            CreatedAt = CreatedAt,
            ExpiresAt = ExpiresAt,
            Notes = Notes,
            Usages = (Usages ?? new Dictionary<string, LicenseUsage>()).ToDictionary(entry => entry.Key, entry => entry.Value.Clone()),
            CustomFields = new Dictionary<int, string>(CustomFields ?? new Dictionary<int, string>())
        };
    }
}
