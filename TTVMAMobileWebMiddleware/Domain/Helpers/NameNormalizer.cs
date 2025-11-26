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
    public static string? NormalizeDOBo(string? dob)
    {
        if (string.IsNullOrWhiteSpace(dob))
            return null;

        var normalized = ToAsciiDigits(dob);
        normalized = StripPunctuation(normalized);
        return normalized.Trim().ToUpperInvariant();
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

