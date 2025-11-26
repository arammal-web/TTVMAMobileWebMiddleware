using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response model for citizen search
/// </summary>
public class CitizenSearchResponse
{
    public string QueryId { get; set; } = null!;
    public List<CitizenCandidate> Candidates { get; set; } = new();
    public SearchAuditInfo Audit { get; set; } = new();
}

/// <summary>
/// Candidate match from search
/// </summary>
public class CitizenCandidate
{
    public int CitizenLocalId { get; set; }
    public double Score { get; set; }
    public string Confidence { get; set; } = null!; // HIGH, MEDIUM, LOW
    public List<string> MatchReasons { get; set; } = new();
    public Dictionary<string, object> FieldsMatched { get; set; } = new();
    public   CitizenABP  citizens   { get; set; } = new();
}

/// <summary>
/// Audit information for search operation
/// </summary>
public class SearchAuditInfo
{
    public int TookMs { get; set; }
    public bool Normalized { get; set; }
    public bool HypocorismApplied { get; set; }
}

