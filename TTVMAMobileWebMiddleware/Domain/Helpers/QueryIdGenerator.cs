using System;
using System.Security.Cryptography;

namespace TTVMAMobileWebMiddleware.Domain.Helpers;

/// <summary>
/// Generates unique query IDs for search operations
/// </summary>
public static class QueryIdGenerator
{
    /// <summary>
    /// Generates a query ID using timestamp and random bytes
    /// Format: q-{timestamp10}-{random16}
    /// </summary>
    public static string GenerateQueryId()
    {
        // Get timestamp in milliseconds (UTC)
        var timestampMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Generate 80 bits of cryptographically secure randomness (10 bytes)
        var randomBytes = new byte[10];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Encode timestamp to 10 chars in Base32 Crockford (simplified)
        var timestampEncoded = Base32CrockfordEncode(timestampMs, 10);

        // Encode randomness to 16 chars
        var randomEncoded = Base32CrockfordEncode(randomBytes, 16);

        return $"q-{timestampEncoded}-{randomEncoded}";
    }

    private static string Base32CrockfordEncode(long value, int width)
    {
        const string chars = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        var result = new System.Text.StringBuilder();
        var num = value;
        
        do
        {
            result.Insert(0, chars[(int)(num % 32)]);
            num /= 32;
        } while (num > 0);

        // Left-pad with zeros to width
        while (result.Length < width)
        {
            result.Insert(0, '0');
        }

        return result.ToString();
    }

    private static string Base32CrockfordEncode(byte[] bytes, int width)
    {
        const string chars = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        var result = new System.Text.StringBuilder();
        
        // Convert bytes to base32
        long value = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            value = (value << 8) | bytes[i];
        }

        do
        {
            result.Insert(0, chars[(int)(value % 32)]);
            value /= 32;
        } while (value > 0);

        // Left-pad with zeros to width
        while (result.Length < width)
        {
            result.Insert(0, '0');
        }

        return result.ToString();
    }
}

