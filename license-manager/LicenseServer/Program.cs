using System.Linq;
using LicenseManager.Models;
using LicenseManager.Services;
using LicenseServer.Models;
using Microsoft.AspNetCore.Http;
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
            var dataPath = ResolvePath(options.DataPath, "licenses.json");
            return new LicenseRepository(dataPath);
        });
        builder.Services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<LicenseServerOptions>>().Value;
            var auditPath = ResolvePath(options.AuditPath, "audit.log");
            return new AuditService(auditPath);
        });
        builder.Services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<LicenseServerOptions>>().Value;
            var userPath = ResolvePath(options.UserStorePath, "users.json");
            return new AuthService(userPath);
        });
        builder.Services.AddSingleton<SessionManager>();
        builder.Services.AddSingleton<LicenseResponseEncoder>();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapGet("/apiversion.php", (IOptions<LicenseServerOptions> options) =>
            Results.Text(options.Value.ApiVersion, "text/plain"));

        app.MapGet("/version.php", (IOptions<LicenseServerOptions> options) =>
            Results.Text(options.Value.CmsVersion, "text/plain"));

        app.MapGet("/applications/nexus/interface/licenses", HandleLicenseRequest);
        app.MapGet("/applications/nexus/interface/licenses/", HandleLicenseRequest);

        app.MapGet("/admin", () => Results.Redirect("/admin/index.html"));

        app.MapPost("/admin/api/login", (LoginRequest request, AuthService authService,
            SessionManager sessions, AuditService audit) =>
        {
            var user = authService.Authenticate(request.Username, request.Password);
            if (user is null)
            {
                return Results.Json(new { message = "Credenciais inválidas." },
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var token = sessions.CreateSession(user);
            audit.Record("Login (web)", user.Username, details: "Sessão autenticada pelo painel web.");
            return Results.Json(new { token, username = user.Username });
        });

        app.MapPost("/admin/api/logout", (HttpContext context, SessionManager sessions, AuditService audit) =>
        {
            var token = ExtractToken(context);
            var session = sessions.Validate(token);
            if (session is not null)
            {
                audit.Record("Logout (web)", session.User.Username, details: "Sessão encerrada pelo painel web.");
            }

            sessions.Invalidate(token);
            return Results.Ok();
        });

        app.MapGet("/admin/api/licenses", (HttpContext context, SessionManager sessions,
            LicenseRepository repository) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            var licenses = repository.Load().OrderByDescending(l => l.CreatedAt);
            return Results.Json(licenses);
        });

        app.MapPost("/admin/api/licenses", (HttpContext context, SessionManager sessions,
            LicenseRepository repository, AuditService audit, LicenseInput input) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            if (string.IsNullOrWhiteSpace(input.Key) || string.IsNullOrWhiteSpace(input.Email))
            {
                return Results.Json(new { message = "Chave e e-mail são obrigatórios." },
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var license = new License
            {
                Key = input.Key.Trim(),
                Email = input.Email.Trim(),
                Type = input.Type,
                Status = input.Status,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = NormalizeDate(input.ExpiresAt),
                Notes = input.Notes?.Trim() ?? string.Empty
            };

            repository.Upsert(license);
            audit.Record("Criação de licença (web)", session.User.Username, license,
                "Licença criada via painel web.");
            return Results.Json(license);
        });

        app.MapPut("/admin/api/licenses/{id:guid}", (HttpContext context, Guid id,
            LicenseInput input, SessionManager sessions, LicenseRepository repository,
            AuditService audit) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            var existing = repository.FindById(id);
            if (existing is null)
            {
                return Results.Json(new { message = "Licença não encontrada." },
                    statusCode: StatusCodes.Status404NotFound);
            }

            if (string.IsNullOrWhiteSpace(input.Key) || string.IsNullOrWhiteSpace(input.Email))
            {
                return Results.Json(new { message = "Chave e e-mail são obrigatórios." },
                    statusCode: StatusCodes.Status400BadRequest);
            }

            existing.Key = input.Key.Trim();
            existing.Email = input.Email.Trim();
            existing.Type = input.Type;
            existing.Status = input.Status;
            existing.ExpiresAt = NormalizeDate(input.ExpiresAt);
            existing.Notes = input.Notes?.Trim() ?? string.Empty;

            repository.Upsert(existing);
            audit.Record("Atualização de licença (web)", session.User.Username, existing,
                "Licença atualizada via painel web.");
            return Results.Json(existing);
        });

        app.MapPost("/admin/api/licenses/{id:guid}/status", (HttpContext context, Guid id,
            StatusUpdateRequest request, SessionManager sessions, LicenseRepository repository,
            AuditService audit) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            var existing = repository.FindById(id);
            if (existing is null)
            {
                return Results.Json(new { message = "Licença não encontrada." },
                    statusCode: StatusCodes.Status404NotFound);
            }

            existing.Status = request.Status;
            repository.Upsert(existing);
            audit.Record("Alteração de status (web)", session.User.Username, existing,
                $"Status alterado para {existing.Status} via painel web.");
            return Results.Json(existing);
        });

        app.MapDelete("/admin/api/licenses/{id:guid}", (HttpContext context, Guid id,
            SessionManager sessions, LicenseRepository repository, AuditService audit) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            var existing = repository.FindById(id);
            if (existing is null)
            {
                return Results.Json(new { message = "Licença não encontrada." },
                    statusCode: StatusCodes.Status404NotFound);
            }

            repository.Delete(id);
            audit.Record("Exclusão de licença (web)", session.User.Username, existing,
                "Licença excluída via painel web.");
            return Results.Ok();
        });

        app.MapGet("/admin/api/audit", (HttpContext context, SessionManager sessions,
            AuditService audit) =>
        {
            var session = GetSession(context, sessions);
            if (session is null)
            {
                return UnauthorizedResponse();
            }

            var limit = 50;
            if (context.Request.Query.TryGetValue("limit", out var values) &&
                int.TryParse(values, out var parsed))
            {
                limit = Math.Clamp(parsed, 1, 500);
            }

            var entries = audit.GetEntries(limit);
            return Results.Json(entries);
        });

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

    private static string ResolvePath(string? configuredPath, string defaultFileName)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.Combine(AppContext.BaseDirectory, defaultFileName);
        }

        if (Path.IsPathRooted(configuredPath))
        {
            return configuredPath;
        }

        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuredPath));
    }

    private static SessionInfo? GetSession(HttpContext context, SessionManager sessions)
    {
        var token = ExtractToken(context);
        return sessions.Validate(token);
    }

    private static string? ExtractToken(HttpContext context)
    {
        var token = context.Request.Headers["X-Auth-Token"].ToString();
        if (!string.IsNullOrWhiteSpace(token))
        {
            return token;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            var raw = authorization.ToString();
            const string bearerPrefix = "Bearer ";
            if (raw.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                token = raw[bearerPrefix.Length..].Trim();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return token;
                }
            }
        }

        if (context.Request.Query.TryGetValue("token", out var queryValue))
        {
            token = queryValue.ToString();
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }
        }

        return null;
    }

    private static DateTime? NormalizeDate(DateTime? value)
    {
        if (value is null)
        {
            return null;
        }

        if (value.Value.Kind == DateTimeKind.Unspecified)
        {
            return DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        }

        return value.Value.ToUniversalTime();
    }

    private static IResult UnauthorizedResponse()
    {
        return Results.Json(new { message = "Autenticação obrigatória." },
            statusCode: StatusCodes.Status401Unauthorized);
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
