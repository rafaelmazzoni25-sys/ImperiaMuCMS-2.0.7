using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LicenseServer;

public class LicenseResponseEncoder
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public LicenseResponseEncoder(IOptions<LicenseServerOptions> optionsAccessor)
    {
        var options = optionsAccessor.Value;
        _key = BuildKey(options.SecretKey);
        _iv = BuildIv(options.SecretIv);
    }

    public IResult Encode(IDictionary<string, object?> payload)
    {
        var json = JsonSerializer.Serialize(payload, _serializerOptions);
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = _key;
        aes.IV = _iv;
        using var encryptor = aes.CreateEncryptor();
        var plaintext = Encoding.UTF8.GetBytes(json);
        var encrypted = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
        return Results.Text(Convert.ToBase64String(encrypted), "text/plain");
    }

    private static byte[] BuildKey(string secret)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(secret ?? string.Empty));
        var hex = Convert.ToHexString(hash).ToLowerInvariant();
        return Encoding.UTF8.GetBytes(hex[..32]);
    }

    private static byte[] BuildIv(string secretIv)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(secretIv ?? string.Empty));
        var hex = Convert.ToHexString(hash).ToLowerInvariant();
        return Encoding.UTF8.GetBytes(hex[..16]);
    }
}
