using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TTVMAMobileWebMiddleware.Domain.Helpers;

/// <summary>
/// Normalizes Arabic and Latin names for matching
/// </summary>
public static class NameNormalizer
{
    // Arabic-Indic and Extended Arabic-Indic digits → ASCII
    private static readonly Dictionary<char, char> DigitMap = new()
    {
        { '٠', '0' }, { '١', '1' }, { '٢', '2' }, { '٣', '3' }, { '٤', '4' },
        { '٥', '5' }, { '٦', '6' }, { '٧', '7' }, { '٨', '8' }, { '٩', '9' },
        { '۰', '0' }, { '۱', '1' }, { '۲', '2' }, { '۳', '3' }, { '۴', '4' },
        { '۵', '5' }, { '۶', '6' }, { '۷', '7' }, { '۸', '8' }, { '۹', '9' }
    };

    /// <summary>
    /// Normalizes Arabic name: removes diacritics, normalizes alef/ya variants, removes tatweel
    /// </summary>
    public static string? NormalizeArabicName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var normalized = name;

        // Convert Arabic digits to ASCII
        normalized = ToAsciiDigits(normalized);

        // Remove tatweel (U+0640)
        normalized = normalized.Replace("\u0640", "");

        // Remove Arabic diacritics (harakat)
        normalized = RemoveArabicDiacritics(normalized);

        // Normalize Alef variants (أ, إ, آ, ا, ٱ) → ا
        normalized = normalized.Replace("أ", "ا")
                                .Replace("إ", "ا")
                                .Replace("آ", "ا")
                                .Replace("ٱ", "ا");

        // Normalize Ya variants (ى, ي) → ي
        normalized = normalized.Replace("ى", "ي");

        // Normalize Hamza (conservative)
        normalized = normalized.Replace("ئ", "ي")
                                .Replace("ؤ", "و")
                                .Replace("ء", "");

        // Normalize Teh Marbuta (ة) → ه (optional, some prefer keeping ة)
        // normalized = normalized.Replace("ة", "ه");

        // Remove punctuation
        normalized = StripPunctuation(normalized);

        // Collapse whitespace
        normalized = CollapseWhitespace(normalized);

        return normalized.Trim();
    }

    /// <summary>
    /// Normalizes Latin name: case folding, removes diacritics, removes punctuation
    /// </summary>
    public static string? NormalizeLatinName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var normalized = name;

        // Convert digits to ASCII
        normalized = ToAsciiDigits(normalized);

        // Unicode case folding (to lowercase)
        normalized = normalized.ToLowerInvariant();

        // Remove Latin diacritics (NFD decomposition + remove combining marks)
        normalized = RemoveDiacritics(normalized);

        // Remove punctuation
        normalized = StripPunctuation(normalized);

        // Collapse whitespace
        normalized = CollapseWhitespace(normalized);

        return normalized.Trim();
    }

    /// <summary>
    /// Normalizes phone number to E.164 format
    /// </summary>
    public static string? NormalizePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        // Extract only digits
        var digits = new string(phone.Where(char.IsDigit).ToArray());

        if (string.IsNullOrEmpty(digits))
            return null;

        // If starts with "00", replace with "+"
        if (digits.StartsWith("00"))
        {
            return "+" + digits.Substring(2);
        }

        // If starts with "+", ensure it's followed by digits
        if (phone.TrimStart().StartsWith("+"))
        {
            return "+" + digits;
        }

        // Lebanon-specific: if starts with "0", add "+961"
        if (digits.StartsWith("0") && digits.Length > 1)
        {
            return "+961" + digits.Substring(1);
        }

        // If no country code, assume Lebanon (+961)
        if (!digits.StartsWith("961"))
        {
            return "+961" + digits;
        }

        return "+" + digits;
    }

    /// <summary>
    /// Normalizes registration number
    /// </summary>
    public static string? NormalizeRegistrationNo(string? regNo)
    {
        if (string.IsNullOrWhiteSpace(regNo))
            return null;

        var normalized = ToAsciiDigits(regNo);
        normalized = normalized.ToUpperInvariant();
        normalized = Regex.Replace(normalized, @"[^A-Z0-9\-\s]", "");
        normalized = CollapseWhitespace(normalized);
        normalized = normalized.Replace(" - ", "-");

        return normalized.Trim();
    }

    /// <summary>
    /// Normalizes document numbers (National ID, Passport)
    /// </summary>
    public static string? NormalizeDocumentNo(string? docNo)
    {
        if (string.IsNullOrWhiteSpace(docNo))
            return null;

        var normalized = ToAsciiDigits(docNo);
        normalized = StripPunctuation(normalized);
        return normalized.Trim().ToUpperInvariant();
    }
    /// <summary>
    /// Normalizes date of birth to YYYY-MM-DD format
    /// Accepts common formats: YYYY/MM/DD, DD-MM-YYYY, YYYY-MM-DD, etc.
    /// </summary>
    public static string? NormalizeDateOfBirth(string? dob)
    {
        if (string.IsNullOrWhiteSpace(dob))
            return null;

        // Convert Arabic digits to ASCII
        var normalized = ToAsciiDigits(dob);
        normalized = normalized.Trim();

        // Try parsing with common date formats
        var dateFormats = new[]
        {
            "yyyy-MM-dd",
            "yyyy/MM/dd",
            "dd-MM-yyyy",
            "dd/MM/yyyy",
            "MM/dd/yyyy",
            "MM-dd-yyyy",
            "yyyy.MM.dd",
            "dd.MM.yyyy",
            "dd MMM yyyy", // e.g., "12 Apr 1995"
            "dd MMMM yyyy", // e.g., "12 April 1995"
            "MMM dd, yyyy", // e.g., "Apr 12, 1995"
            "MMMM dd, yyyy" // e.g., "April 12, 1995"
        };

        // Try parsing with invariant culture first
        if (DateTime.TryParseExact(normalized, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate.ToString("yyyy-MM-dd");
        }

        // Try lenient parsing
        if (DateTime.TryParse(normalized, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return parsedDate.ToString("yyyy-MM-dd");
        }

        // If parsing fails, try to extract date parts from common patterns
        // Pattern: YYYYMMDD or DDMMYYYY (8 digits)
        var digitsOnly = new string(normalized.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length == 8)
        {
            // Try YYYYMMDD format first
            if (int.TryParse(digitsOnly.Substring(0, 4), out var year) &&
                int.TryParse(digitsOnly.Substring(4, 2), out var month) &&
                int.TryParse(digitsOnly.Substring(6, 2), out var day) &&
                year >= 1900 && year <= 2100 &&
                month >= 1 && month <= 12 &&
                day >= 1 && day <= 31)
            {
                try
                {
                    var date = new DateTime(year, month, day);
                    return date.ToString("yyyy-MM-dd");
                }
                catch
                {
                    // Invalid date, try DDMMYYYY format
                }
            }

            // Try DDMMYYYY format
            if (int.TryParse(digitsOnly.Substring(0, 2), out var day2) &&
                int.TryParse(digitsOnly.Substring(2, 2), out var month2) &&
                int.TryParse(digitsOnly.Substring(4, 4), out var year2) &&
                year2 >= 1900 && year2 <= 2100 &&
                month2 >= 1 && month2 <= 12 &&
                day2 >= 1 && day2 <= 31)
            {
                try
                {
                    var date = new DateTime(year2, month2, day2);
                    return date.ToString("yyyy-MM-dd");
                }
                catch
                {
                    // Invalid date
                }
            }
        }

        // If all parsing fails, return null
        return null;
    }

    /// <summary>
    /// Legacy method name - redirects to NormalizeDateOfBirth
    /// </summary>
    [Obsolete("Use NormalizeDateOfBirth instead")]
    public static string? NormalizeDOBo(string? dob)
    {
        return NormalizeDateOfBirth(dob);
    }

    private static string ToAsciiDigits(string input)
    {
        var sb = new StringBuilder();
        foreach (var ch in input)
        {
            sb.Append(DigitMap.TryGetValue(ch, out var ascii) ? ascii : ch);
        }
        return sb.ToString();
    }

    private static string RemoveArabicDiacritics(string input)
    {
        // Remove Arabic diacritics (harakat): 064B-065F, 0670, etc.
        var sb = new StringBuilder();
        foreach (var ch in input)
        {
            var codePoint = (int)ch;
            // Skip diacritic marks
            if (codePoint >= 0x064B && codePoint <= 0x065F)
                continue;
            if (codePoint == 0x0670)
                continue;
            sb.Append(ch);
        }
        return sb.ToString();
    }

    private static string RemoveDiacritics(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (category != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string StripPunctuation(string input)
    {
        return Regex.Replace(input, @"[^\w\s\-]", "");
    }

    private static string CollapseWhitespace(string input)
    {
        return Regex.Replace(input, @"\s+", " ");
    }
}

