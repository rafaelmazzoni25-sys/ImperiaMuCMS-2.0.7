namespace LicenseServer;

public class LicenseServerOptions
{
    public string DataPath { get; set; } = "../LicenseManager/licenses.json";

    public string SecretKey { get; set; }
        = "XFmva8nIbtoV88dzQoioafgZlipk9dBNhU4nEeS3SHH94LkdES58ThOozVjG0wFdeLPE3ZUhIKMkCPWAn17XzJzQ1Ax3K0zzu2AP2BsxbwLi8HJI73IjkkVAUSphN87Wsxd7cKi8zqSxUIzbe2otwHvVeZH6UhL7yFepgnx0BumReJ2gfAQdAwY8VvS3LBfz5SysoUHlJUuIli7HeuePjtyC6lrfuo1lz6lxKqaCBGecoJNeGoYflkEBJNmkoIF9";

    public string SecretIv { get; set; }
        = "xk3xudsF8XjuItROFaMuiDcPHdB0VhCpFx09glr02rO98zcTtT1lmKATtHEeiuKH";

    public string ApiVersion { get; set; } = "1";

    public string CmsVersion { get; set; } = "2.0.7";

    public string? UserStorePath { get; set; } = "../LicenseManager/users.json";

    public string? AuditPath { get; set; } = "../LicenseManager/audit.log";
}
