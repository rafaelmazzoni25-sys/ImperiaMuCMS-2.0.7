using System;
using System.Collections.Generic;
using System.Globalization;
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
        var directory = Path.GetDirectoryName(_storagePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        CreateBackupIfNeeded();

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

    public License? FindById(Guid id)
    {
        return Load().FirstOrDefault(l => l.Id == id);
    }

    public License? FindByKeyAndEmail(string key, string email)
    {
        return Load().FirstOrDefault(license =>
            string.Equals(license.Key, key, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(license.Email, email, StringComparison.OrdinalIgnoreCase));
    }

    private void CreateBackupIfNeeded()
    {
        if (!File.Exists(_storagePath))
        {
            return;
        }

        var directory = Path.GetDirectoryName(_storagePath);
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        var backupDirectory = Path.Combine(directory, "backups");
        Directory.CreateDirectory(backupDirectory);

        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
        var fileName = Path.GetFileName(_storagePath);
        var backupFile = Path.Combine(backupDirectory, $"{timestamp}-{fileName}");

        File.Copy(_storagePath, backupFile, overwrite: true);

        CleanupOldBackups(backupDirectory, fileName, 20);
    }

    private static void CleanupOldBackups(string backupDirectory, string originalFileName, int maxBackups)
    {
        if (!Directory.Exists(backupDirectory))
        {
            return;
        }

        var files = Directory.GetFiles(backupDirectory, $"*-{originalFileName}")
            .OrderByDescending(File.GetCreationTimeUtc)
            .ToList();

        for (var index = maxBackups; index < files.Count; index++)
        {
            try
            {
                File.Delete(files[index]);
            }
            catch
            {
                // Ignore cleanup failures to avoid interrupting the save process.
            }
        }
    }
}
