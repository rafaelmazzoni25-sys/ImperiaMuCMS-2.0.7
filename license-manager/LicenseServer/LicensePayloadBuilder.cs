using System;
using System.Collections.Generic;
using System.Linq;

using LicenseManager.Models;

namespace LicenseServer;

internal static class LicensePayloadBuilder
{
    public static IDictionary<string, object?> BuildSuccessPayload(License license)
    {
        return new Dictionary<string, object?>
        {
            ["response"] = "OKAY",
            ["status"] = MapStatus(license.Status),
            ["key"] = license.Key,
            ["identifier"] = license.Email,
            ["purchase_name"] = license.PurchaseName,
            ["custom_fields"] = BuildCustomFields(license),
            ["expires"] = license.ExpiresAt.HasValue
                ? new DateTimeOffset(license.ExpiresAt.Value).ToUnixTimeSeconds()
                : 0
        };
    }

    public static IDictionary<string, object?> BuildErrorPayload(string status, string message, int code)
    {
        return new Dictionary<string, object?>
        {
            ["response"] = "ERROR",
            ["status"] = status,
            ["error_code"] = code,
            ["message"] = message,
            ["custom_fields"] = Enumerable.Repeat(string.Empty, 21).ToArray()
        };
    }

    public static IDictionary<string, object?> BuildUsageError(License license, string message, int code)
    {
        return new Dictionary<string, object?>
        {
            ["response"] = "ERROR",
            ["status"] = MapStatus(license.Status),
            ["error_code"] = code,
            ["message"] = message,
            ["custom_fields"] = BuildCustomFields(license)
        };
    }

    private static string MapStatus(LicenseStatus status)
    {
        return status switch
        {
            LicenseStatus.Active => "ACTIVE",
            LicenseStatus.Inactive => "INACTIVE",
            LicenseStatus.Banned => "BANNED",
            _ => "INACTIVE"
        };
    }

    private static string[] BuildCustomFields(License license)
    {
        var fields = Enumerable.Repeat(string.Empty, 21).ToArray();
        foreach (var entry in license.CustomFields)
        {
            if (entry.Key >= 0 && entry.Key < fields.Length)
            {
                fields[entry.Key] = entry.Value ?? string.Empty;
            }
        }

        if (string.IsNullOrWhiteSpace(fields[20]))
        {
            fields[20] = license.LicenseTypeSlug;
        }

        return fields;
    }
}
