using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LicenseManager.Models;

namespace LicenseManager.Services;

public class AuthService
{
    private readonly string _usersPath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly object _syncRoot = new();

    public AuthService(string? usersPath = null)
    {
        _usersPath = usersPath ?? Path.Combine(AppContext.BaseDirectory, "users.json");
        EnsureSeedUser();
    }

    public User? Authenticate(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var users = LoadUsers();
        var user = users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            return null;
        }

        return VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public IReadOnlyList<User> LoadUsers()
    {
        lock (_syncRoot)
        {
            if (!File.Exists(_usersPath))
            {
                return Array.Empty<User>();
            }

            using var stream = File.OpenRead(_usersPath);
            return JsonSerializer.Deserialize<List<User>>(stream, _jsonOptions) ?? new List<User>();
        }
    }

    public void SaveUsers(IEnumerable<User> users)
    {
        lock (_syncRoot)
        {
            var directory = Path.GetDirectoryName(_usersPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = File.Create(_usersPath);
            JsonSerializer.Serialize(stream, users, _jsonOptions);
        }
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var builder = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            builder.AppendFormat("{0:x2}", b);
        }

        return builder.ToString();
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var hashed = HashPassword(password);
        return string.Equals(hashed, storedHash, StringComparison.OrdinalIgnoreCase);
    }

    private void EnsureSeedUser()
    {
        lock (_syncRoot)
        {
            if (File.Exists(_usersPath))
            {
                return;
            }

            var defaultUser = new User
            {
                Username = "admin",
                PasswordHash = HashPassword("admin"),
                Roles = new List<string> { "Administrator" }
            };

            SaveUsers(new[] { defaultUser });
        }
    }
}
