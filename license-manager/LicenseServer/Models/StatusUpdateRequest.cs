using LicenseManager.Models;

namespace LicenseServer.Models;

public class StatusUpdateRequest
{
    public LicenseStatus Status { get; set; }
        = LicenseStatus.Active;
}
