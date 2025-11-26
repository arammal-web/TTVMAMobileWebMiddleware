using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Application.Interfaces;
/// <summary>
/// Reads configuration values from [dbo].[AppSettings].
/// A ParamCode may store values in ParamValue and/or ParamValue1..4.
/// </summary>
    public class AppSettingService : IAppSettingService
    {
    private readonly DLSDbContext _context;

    public AppSettingService(DLSDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
             
    }

    // -------------------------------
    // Core: entity and value retrieval
    // -------------------------------

    public async Task<AppSetting?> GetByCodeAsync(string paramCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(paramCode)) return null;

        return await _context.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.ParamCode == paramCode &&
                // if you later add soft delete columns, add their filter here
                true, ct);
    }

    public async Task<string?> GetValueAsync(string paramCode, CancellationToken ct = default)
    {
        var s = await GetByCodeAsync(paramCode, ct);
        return FirstNonEmptyValue(s);
    }

    public async Task<string[]?> GetValuesAsync(string paramCode, CancellationToken ct = default)
    {
        var s = await GetByCodeAsync(paramCode, ct);
        var values = AllNonEmptyValues(s);
        return values.Length == 0 ? null : values;
    }
     
    public async Task<T?> GetValueAsync<T>(string paramCode, CancellationToken ct = default)
    {
        var raw = await GetValueAsync(paramCode, ct);
        if (string.IsNullOrWhiteSpace(raw)) return default;

        // unwrap Nullable<T> if present
        var targetType = typeof(T);
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        object? result = null;

        if (underlying == typeof(string))
        {
            result = raw;
        }
        else if (underlying == typeof(bool))
        {
            if (TryParseBool(raw, out var b)) result = b;
            else if (raw == "1") result = true;     // common cases
            else if (raw == "0") result = false;
        }
        else if (underlying == typeof(int))
        {
            if (int.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var i)) result = i;
        }
        else if (underlying == typeof(double))
        {
            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) result = d;
        }
        else if (underlying == typeof(DateTime))
        {
            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt)) result = dt;
        }
        else if (underlying.IsEnum)
        {
            if (Enum.TryParse(underlying, raw, true, out var e)) result = e;
            else if (int.TryParse(raw, out var enumInt)) result = Enum.ToObject(underlying, enumInt);
        }

        if (result != null)
            return (T?)result;

        try
        {
            // final attempt: convert to the underlying (non-nullable) type
            var converted = Convert.ChangeType(raw, underlying, CultureInfo.InvariantCulture);
            return (T?)converted;
        }
        catch
        {
            return default;
        }
    }


// ------------------------
// Convenience/Named getters
// ------------------------

public async Task<string> GetAIUrlAsync(CancellationToken ct = default)
        => await GetValueAsync("AI-URL", ct) ?? string.Empty;

    public Task<int?> GetNumberOfTryAsync(CancellationToken ct = default)
        => GetValueAsync<int?>("NumberOfTry", ct);

    public Task<int?> GetApplicationValidityWindowAsync(CancellationToken ct = default)
        => GetValueAsync<int?>("ApplicationValidityWindow", ct);

    public Task<bool?> GetExamListAssignNowDefaultAsync(CancellationToken ct = default)
        => GetValueAsync<bool?>("ExamList_AssignNowDefault", ct);

    public Task<int?> GetExamListDefaultMaxSeatsAsync(CancellationToken ct = default)
        => GetValueAsync<int?>("ExamList_DefaultMaxSeats", ct);

    // -------------
    // Local helpers
    // -------------

    private static string? FirstNonEmptyValue(AppSetting? s)
        => AllNonEmptyValues(s).FirstOrDefault();

    private static string[] AllNonEmptyValues(AppSetting? s)
    {
        if (s == null) return Array.Empty<string>();
        var list = new List<string?> { s.ParamValue, s.ParamValue1, s.ParamValue2, s.ParamValue3, s.ParamValue4 };
        return list
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v!.Trim())
            .ToArray();
    }

    private static bool TryParseBool(string value, out bool result)
    {
        // Accept common truthy/falsy variants:
        // true/false, 1/0, yes/no, y/n, on/off
        var v = value.Trim().ToLowerInvariant();
        switch (v)
        {
            case "1":
            case "true":
            case "yes":
            case "y":
            case "on":
                result = true; return true;
            case "0":
            case "false":
            case "no":
            case "n":
            case "off":
                result = false; return true;
        }
        return bool.TryParse(value, out result);
    }
} 
