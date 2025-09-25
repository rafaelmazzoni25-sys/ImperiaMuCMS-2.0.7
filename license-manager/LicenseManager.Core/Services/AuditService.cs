using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LicenseManager.Models;

namespace LicenseManager.Services;

public class AuditService
{
    private readonly string _auditPath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly object _syncRoot = new();

    public AuditService(string? auditPath = null)
    {
        _auditPath = auditPath ?? Path.Combine(AppContext.BaseDirectory, "audit.log");
    }

    public void Record(string action, string performedBy, License? license = null, string? details = null)
    {
        var entry = new AuditEntry
        {
            Action = action,
            PerformedBy = performedBy,
            LicenseId = license?.Id,
            LicenseKey = license?.Key,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        lock (_syncRoot)
        {
            var entries = LoadEntriesInternal();
            entries.Add(entry);
            SaveEntries(entries);
        }
    }

    public IReadOnlyList<AuditEntry> GetEntries(int limit = 100)
    {
        lock (_syncRoot)
        {
            var entries = LoadEntriesInternal()
                .OrderByDescending(e => e.Timestamp)
                .Take(Math.Max(1, limit))
                .ToList();
            return entries;
        }
    }

    private List<AuditEntry> LoadEntriesInternal()
    {
        if (!File.Exists(_auditPath))
        {
            return new List<AuditEntry>();
        }

        using var stream = File.OpenRead(_auditPath);
        return JsonSerializer.Deserialize<List<AuditEntry>>(stream, _jsonOptions) ?? new List<AuditEntry>();
    }

    private void SaveEntries(IEnumerable<AuditEntry> entries)
    {
        var directory = Path.GetDirectoryName(_auditPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var stream = File.Create(_auditPath);
        JsonSerializer.Serialize(stream, entries, _jsonOptions);
    }
}
