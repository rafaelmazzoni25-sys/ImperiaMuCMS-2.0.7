using System;
using System.Security.Cryptography;
using System.Text.Json;

namespace LicenseServer;

internal static class LicenseUtilities
{
    public static string GenerateUsageId()
    {
        Span<byte> buffer = stackalloc byte[8];
        RandomNumberGenerator.Fill(buffer);
        return Convert.ToHexString(buffer).ToLowerInvariant();
    }

    public static string ExtractDomain(string? extra)
    {
        if (string.IsNullOrWhiteSpace(extra))
        {
            return string.Empty;
        }

        try
        {
            using var document = JsonDocument.Parse(extra);
            if (document.RootElement.TryGetProperty("url", out var urlElement) &&
                urlElement.ValueKind == JsonValueKind.String)
            {
                return urlElement.GetString() ?? string.Empty;
            }
        }
        catch (JsonException)
        {
            // ignore malformed payloads
        }

        return string.Empty;
    }
}
