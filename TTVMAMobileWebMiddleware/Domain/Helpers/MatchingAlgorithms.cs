using System;
using System.Linq;

namespace TTVMAMobileWebMiddleware.Domain.Helpers;

/// <summary>
/// Matching algorithms for citizen search
/// </summary>
public static class MatchingAlgorithms
{
    /// <summary>
    /// Calculates Jaro-Winkler similarity between two strings
    /// </summary>
    public static double JaroWinklerSimilarity(string? s1, string? s2)
    {
        if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
            return 0.0;

        if (s1 == s2)
            return 1.0;

        var jaro = JaroSimilarity(s1, s2);
        var prefixLength = CommonPrefixLength(s1, s2, 4);
        return jaro + (0.1 * prefixLength * (1 - jaro));
    }

    /// <summary>
    /// Calculates Jaro similarity
    /// </summary>
    private static double JaroSimilarity(string s1, string s2)
    {
        if (s1 == s2)
            return 1.0;

        int matchWindow = Math.Max(s1.Length, s2.Length) / 2 - 1;
        if (matchWindow < 0)
            matchWindow = 0;

        var s1Matches = new bool[s1.Length];
        var s2Matches = new bool[s2.Length];

        int matches = 0;
        int transpositions = 0;

        // Find matches
        for (int i = 0; i < s1.Length; i++)
        {
            int start = Math.Max(0, i - matchWindow);
            int end = Math.Min(i + matchWindow + 1, s2.Length);

            for (int j = start; j < end; j++)
            {
                if (s2Matches[j] || s1[i] != s2[j])
                    continue;
                s1Matches[i] = true;
                s2Matches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0)
            return 0.0;

        // Find transpositions
        int k = 0;
        for (int i = 0; i < s1.Length; i++)
        {
            if (!s1Matches[i])
                continue;
            while (!s2Matches[k])
                k++;
            if (s1[i] != s2[k])
                transpositions++;
            k++;
        }

        return (matches / (double)s1.Length +
                matches / (double)s2.Length +
                (matches - transpositions / 2.0) / matches) / 3.0;
    }

    /// <summary>
    /// Calculates common prefix length (capped at maxLength)
    /// </summary>
    private static int CommonPrefixLength(string s1, string s2, int maxLength)
    {
        int prefixLength = 0;
        int max = Math.Min(Math.Min(s1.Length, s2.Length), maxLength);
        for (int i = 0; i < max; i++)
        {
            if (s1[i] == s2[i])
                prefixLength++;
            else
                break;
        }
        return prefixLength;
    }

    /// <summary>
    /// Calculates normalized Levenshtein distance (similarity score)
    /// </summary>
    public static double LevenshteinSimilarity(string? s1, string? s2)
    {
        if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
            return 0.0;

        if (s1 == s2)
            return 1.0;

        int maxLength = Math.Max(s1.Length, s2.Length);
        if (maxLength == 0)
            return 1.0;

        int distance = LevenshteinDistance(s1, s2);
        return 1.0 - (distance / (double)maxLength);
    }

    /// <summary>
    /// Calculates Levenshtein distance
    /// </summary>
    private static int LevenshteinDistance(string s1, string s2)
    {
        int n = s1.Length;
        int m = s2.Length;

        if (n == 0)
            return m;
        if (m == 0)
            return n;

        var d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
            d[i, 0] = i;
        for (int j = 0; j <= m; j++)
            d[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (s2[j - 1] == s1[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[n, m];
    }

    /// <summary>
    /// Scores Arabic name triplet (first + father + last)
    /// </summary>
    public static double TripletScoreAr(
        string? cFirst, string? cFather, string? cLast,
        string? qFirst, string? qFather, string? qLast)
    {
        double score = 0.0;
        double weight = 1.0 / 3.0;

        if (!string.IsNullOrWhiteSpace(cFirst) && !string.IsNullOrWhiteSpace(qFirst))
        {
            var sim = JaroWinklerSimilarity(cFirst, qFirst);
            score += sim * weight;
        }

        if (!string.IsNullOrWhiteSpace(cFather) && !string.IsNullOrWhiteSpace(qFather))
        {
            var sim = JaroWinklerSimilarity(cFather, qFather);
            score += sim * weight;
        }

        if (!string.IsNullOrWhiteSpace(cLast) && !string.IsNullOrWhiteSpace(qLast))
        {
            var sim = JaroWinklerSimilarity(cLast, qLast);
            score += sim * weight;
        }

        return score;
    }

    /// <summary>
    /// Scores Latin name pair (first + last)
    /// </summary>
    public static double PairScoreEn(
        string? cFirst, string? cLast,
        string? qFirst, string? qLast)
    {
        double score = 0.0;
        double weight = 0.5;

        if (!string.IsNullOrWhiteSpace(cFirst) && !string.IsNullOrWhiteSpace(qFirst))
        {
            var sim = JaroWinklerSimilarity(cFirst, qFirst);
            score += sim * weight;
        }

        if (!string.IsNullOrWhiteSpace(cLast) && !string.IsNullOrWhiteSpace(qLast))
        {
            var sim = JaroWinklerSimilarity(cLast, qLast);
            score += sim * weight;
        }

        return score;
    }

    /// <summary>
    /// Checks if two dates are equal (ignoring time)
    /// </summary>
    public static bool DateEqual(DateTime? d1, DateTime? d2)
    {
        if (!d1.HasValue || !d2.HasValue)
            return false;
        return d1.Value.Date == d2.Value.Date;
    }
}

