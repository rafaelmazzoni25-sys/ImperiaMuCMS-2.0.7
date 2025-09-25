using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using LicenseManager.Models;

namespace LicenseServer;

public class SessionManager
{
    private readonly ConcurrentDictionary<string, SessionInfo> _sessions = new();
    private readonly TimeSpan _lifetime = TimeSpan.FromHours(8);

    public string CreateSession(User user)
    {
        CleanupExpired();
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToHexString(tokenBytes);
        var session = new SessionInfo(user, DateTime.UtcNow.Add(_lifetime));
        _sessions[token] = session;
        return token;
    }

    public SessionInfo? Validate(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        CleanupExpired();
        if (_sessions.TryGetValue(token, out var session) && session.ExpiresAt > DateTime.UtcNow)
        {
            return session;
        }

        _sessions.TryRemove(token, out _);
        return null;
    }

    public void Invalidate(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        _sessions.TryRemove(token, out _);
    }

    private void CleanupExpired()
    {
        foreach (var entry in _sessions)
        {
            if (entry.Value.ExpiresAt <= DateTime.UtcNow)
            {
                _sessions.TryRemove(entry.Key, out _);
            }
        }
    }
}

public record SessionInfo(User User, DateTime ExpiresAt);
