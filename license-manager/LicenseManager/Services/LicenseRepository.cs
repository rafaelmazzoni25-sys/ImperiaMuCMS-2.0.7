using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LicenseManager.Models;

namespace LicenseManager.Services;

public class LicenseRepository
{
    private readonly string _storagePath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public LicenseRepository(string? storagePath = null)
    {
        _storagePath = storagePath ?? Path.Combine(AppContext.BaseDirectory, "licenses.json");
    }

    public IReadOnlyList<License> Load()
    {
        if (!File.Exists(_storagePath))
        {
            return Array.Empty<License>();
        }

        using var stream = File.OpenRead(_storagePath);
        var licenses = JsonSerializer.Deserialize<List<License>>(stream, _jsonOptions);
        return licenses ?? new List<License>();
    }

    public void Save(IEnumerable<License> licenses)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_storagePath)!);
        using var stream = File.Create(_storagePath);
        JsonSerializer.Serialize(stream, licenses, _jsonOptions);
    }

    public License Upsert(License license)
    {
        var licenses = Load().ToList();
        var existingIndex = licenses.FindIndex(l => l.Id == license.Id);
        if (existingIndex >= 0)
        {
            licenses[existingIndex] = license;
        }
        else
        {
            licenses.Add(license);
        }

        Save(licenses);
        return license;
    }

    public void Delete(Guid id)
    {
        var licenses = Load().Where(l => l.Id != id).ToList();
        Save(licenses);
    }
}
