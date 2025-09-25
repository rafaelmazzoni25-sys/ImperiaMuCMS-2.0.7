using LicenseManager.Models;
using LicenseManager.Services;
using Microsoft.Extensions.Options;

namespace LicenseServer;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        builder.Services.Configure<LicenseServerOptions>(builder.Configuration.GetSection("LicenseServer"));
        builder.Services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<LicenseServerOptions>>().Value;
            var dataPath = ResolveDataPath(options.DataPath);
            return new LicenseRepository(dataPath);
        });
        builder.Services.AddSingleton<LicenseResponseEncoder>();

        var app = builder.Build();

        app.MapGet("/apiversion.php", (IOptions<LicenseServerOptions> options) =>
            Results.Text(options.Value.ApiVersion, "text/plain"));

        app.MapGet("/version.php", (IOptions<LicenseServerOptions> options) =>
            Results.Text(options.Value.CmsVersion, "text/plain"));

        app.MapGet("/applications/nexus/interface/licenses", HandleLicenseRequest);
        app.MapGet("/applications/nexus/interface/licenses/", HandleLicenseRequest);

        app.Run();
    }

    private static IResult HandleLicenseRequest(HttpContext context)
    {
        var repository = context.RequestServices.GetRequiredService<LicenseRepository>();
        var encoder = context.RequestServices.GetRequiredService<LicenseResponseEncoder>();
        var query = context.Request.Query;
        var key = query["key"].ToString();
        var identifier = query["identifier"].ToString();

        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(identifier))
        {
            return encoder.Encode(LicensePayloadBuilder.BuildErrorPayload("INACTIVE",
                "Missing key or identifier", 400));
        }

        var license = repository.FindByKeyAndEmail(key, identifier);
        if (license is null)
        {
            return encoder.Encode(LicensePayloadBuilder.BuildErrorPayload("INACTIVE",
                "License does not exist", 404));
        }

        if (query.ContainsKey("check"))
        {
            var usageId = query["usage_id"].ToString();
            if (string.IsNullOrWhiteSpace(usageId) || !license.Usages.ContainsKey(usageId))
            {
                return encoder.Encode(LicensePayloadBuilder.BuildUsageError(license,
                    "Usage identifier is not active for this license.", 461));
            }

            var payload = LicensePayloadBuilder.BuildSuccessPayload(license);
            payload["usage_id"] = usageId;
            return encoder.Encode(payload);
        }

        if (query.ContainsKey("info"))
        {
            var payload = LicensePayloadBuilder.BuildSuccessPayload(license);
            return encoder.Encode(payload);
        }

        if (query.ContainsKey("activate"))
        {
            if (license.Status != LicenseStatus.Active)
            {
                return encoder.Encode(LicensePayloadBuilder.BuildUsageError(license,
                    "License is not active and cannot be activated.", 462));
            }

            var extraRaw = query["extra"].ToString();
            var domain = LicenseUtilities.ExtractDomain(extraRaw);
            var usageId = FindExistingUsage(license, domain) ?? LicenseUtilities.GenerateUsageId();
            license.Usages[usageId] = new LicenseUsage
            {
                Domain = domain,
                CreatedAt = DateTime.UtcNow
            };

            repository.Upsert(license);

            var payload = LicensePayloadBuilder.BuildSuccessPayload(license);
            payload["response"] = "OKAY";
            payload["usage_id"] = usageId;
            return encoder.Encode(payload);
        }

        return encoder.Encode(LicensePayloadBuilder.BuildErrorPayload("INACTIVE",
            "Unknown request", 400));
    }

    private static string ResolveDataPath(string? configuredPath)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.Combine(AppContext.BaseDirectory, "licenses.json");
        }

        if (Path.IsPathRooted(configuredPath))
        {
            return configuredPath;
        }

        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuredPath));
    }

    private static string? FindExistingUsage(License license, string domain)
    {
        if (string.IsNullOrWhiteSpace(domain) || license.Usages.Count == 0)
        {
            return null;
        }

        foreach (var entry in license.Usages)
        {
            if (!string.IsNullOrWhiteSpace(entry.Value.Domain) &&
                string.Equals(entry.Value.Domain, domain, StringComparison.OrdinalIgnoreCase))
            {
                return entry.Key;
            }
        }

        return null;
    }
}
