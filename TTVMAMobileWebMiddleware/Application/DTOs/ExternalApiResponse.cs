namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response from external API call
/// </summary>
public class ExternalApiResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? ResponseContent { get; set; }
    public string? ErrorMessage { get; set; }
}

