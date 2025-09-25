namespace LicenseManager.Models;

public class LicenseUsage
{
    public string Domain { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public LicenseUsage Clone()
    {
        return new LicenseUsage
        {
            Domain = Domain,
            CreatedAt = CreatedAt
        };
    }
}
