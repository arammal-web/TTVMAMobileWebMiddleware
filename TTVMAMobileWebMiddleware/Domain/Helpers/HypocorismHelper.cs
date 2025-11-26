using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TTVMAMobileWebMiddleware.Domain.Helpers;

/// <summary>
/// Detects and generates hypocoristic name variants (e.g., Lolo/Loulou/Lulu)
/// </summary>
public class HypocorismHelper
{
    private static readonly HashSet<char> VowelsEn = new() { 'a', 'e', 'i', 'o', 'u', 'y' };
    private static readonly string[] OuFamilyEn = { "o", "ou", "u", "oo" };
    private static readonly string[] AeiFamilyEn = { "a", "e", "i" };

    private static readonly Dictionary<string, string[]> AliasEn = new()
    {
        { "lulu", new[] { "loulou", "lolo" } },
        { "loulou", new[] { "lulu", "lolo" } },
        { "lolo", new[] { "loulou", "lulu" } }
    };

    private const int MaxVariants = 12;

    /// <summary>
    /// Builds hypocorism variant set for a normalized name
    /// </summary>
    public static HypocorismSet BuildHypocorismSet(string? firstNameAr, string? firstNameEn)
    {
        var set = new HypocorismSet
        {
            Applied = false,
            NameVariantsAr = new List<string>(),
            NameVariantsEn = new List<string>()
        };

        // Arabic variants
        if (!string.IsNullOrWhiteSpace(firstNameAr))
        {
            var normalizedAr = NameNormalizer.NormalizeArabicName(firstNameAr);
            if (!string.IsNullOrWhiteSpace(normalizedAr))
            {
                var arVariants = MakeArabicReduplicativeVariants(normalizedAr);
                if (arVariants.Count > 0)
                {
                    set.NameVariantsAr = arVariants.Take(MaxVariants).ToList();
                    set.Applied = true;
                }
            }
        }

        // Latin variants
        if (!string.IsNullOrWhiteSpace(firstNameEn))
        {
            var normalizedEn = NameNormalizer.NormalizeLatinName(firstNameEn);
            if (!string.IsNullOrWhiteSpace(normalizedEn))
            {
                var enVariants = MakeLatinReduplicativeVariants(normalizedEn);
                var lower = normalizedEn.ToLowerInvariant();
                
                // Add alias dictionary entries
                if (AliasEn.TryGetValue(lower, out var aliases))
                {
                    // Convert to HashSet to use UnionWith, then convert back to List
                    var enVariantsSet = new HashSet<string>(enVariants);
                    enVariantsSet.UnionWith(aliases);
                    enVariants = enVariantsSet.ToList();
                }

                if (enVariants.Count > 0)
                {
                    set.NameVariantsEn = enVariants.Take(MaxVariants).ToList();
                    set.Applied = true;
                }
            }
        }

        return set;
    }

    private static List<string> MakeLatinReduplicativeVariants(string seedEn)
    {
        var variants = new HashSet<string>();
        var collapsed = CollapseOU(seedEn);
        var baseSyllable = DetectReduplicativeBaseLatin(collapsed);

        if (baseSyllable != null)
        {
            var (pre, nucleus, post) = SplitLatinSyllable(baseSyllable);

            // Generate OU-family variants
            foreach (var v in OuFamilyEn)
            {
                var variantBase = pre + v + post;
                variants.Add(variantBase + variantBase);
            }

            // Optional: AEI family alternations
            foreach (var v in AeiFamilyEn)
            {
                var variantBase = pre + v + post;
                variants.Add(variantBase + variantBase);
            }

            // Include original seed and base
            variants.Add(seedEn.ToLowerInvariant());
            variants.Add(baseSyllable + baseSyllable);
        }
        else if (AliasEn.ContainsKey(seedEn.ToLowerInvariant()))
        {
            // Name is already an alias, add its family
            foreach (var alt in AliasEn[seedEn.ToLowerInvariant()])
            {
                variants.Add(alt);
            }
        }

        return variants.ToList();
    }

    private static List<string> MakeArabicReduplicativeVariants(string seedAr)
    {
        var variants = new HashSet<string>();
        var baseSyllable = DetectReduplicativeBaseArabic(seedAr);

        if (baseSyllable != null)
        {
            // Primary: undiacritized reduplication
            variants.Add(baseSyllable + baseSyllable);

            // Optional: add damma variant (U+064F)
            if (baseSyllable.Length >= 1)
            {
                var withDamma = baseSyllable[0] + "\u064F" + (baseSyllable.Length > 1 ? baseSyllable.Substring(1) : "");
                variants.Add(withDamma + baseSyllable);
                variants.Add(baseSyllable + withDamma);
            }

            // Include original seed
            variants.Add(seedAr);
        }

        return variants.ToList();
    }

    private static string CollapseOU(string s)
    {
        return s.ToLowerInvariant()
                .Replace("ou", "o")
                .Replace("oo", "o");
    }

    private static string? DetectReduplicativeBaseLatin(string s)
    {
        // Check for base lengths 2, 3, 4
        for (int baseLen = 2; baseLen <= 4; baseLen++)
        {
            if (s.Length == baseLen * 2)
            {
                var left = s.Substring(0, baseLen);
                var right = s.Substring(baseLen);
                if (left == right && ContainsVowel(left))
                {
                    return left;
                }
            }
        }
        return null;
    }

    private static string? DetectReduplicativeBaseArabic(string s)
    {
        // Check for base lengths 1, 2, 3
        for (int baseLen = 1; baseLen <= 3; baseLen++)
        {
            if (s.Length == baseLen * 2)
            {
                var left = s.Substring(0, baseLen);
                var right = s.Substring(baseLen);
                if (left == right)
                {
                    return left;
                }
            }
        }
        return null;
    }

    private static bool ContainsVowel(string s)
    {
        return s.Any(ch => VowelsEn.Contains(ch));
    }

    private static (string pre, string nucleus, string post) SplitLatinSyllable(string baseSyllable)
    {
        int firstVowelIndex = -1;
        for (int i = 0; i < baseSyllable.Length; i++)
        {
            if (VowelsEn.Contains(baseSyllable[i]))
            {
                firstVowelIndex = i;
                break;
            }
        }

        if (firstVowelIndex == -1)
        {
            return (baseSyllable, "", "");
        }

        int vowelEnd = firstVowelIndex;
        while (vowelEnd < baseSyllable.Length && VowelsEn.Contains(baseSyllable[vowelEnd]))
        {
            vowelEnd++;
        }

        var pre = baseSyllable.Substring(0, firstVowelIndex);
        var nucleus = baseSyllable.Substring(firstVowelIndex, vowelEnd - firstVowelIndex);
        var post = baseSyllable.Substring(vowelEnd);

        return (pre, nucleus, post);
    }
}

/// <summary>
/// Result of hypocorism detection
/// </summary>
public class HypocorismSet
{
    public bool Applied { get; set; }
    public List<string> NameVariantsAr { get; set; } = new();
    public List<string> NameVariantsEn { get; set; } = new();
}

