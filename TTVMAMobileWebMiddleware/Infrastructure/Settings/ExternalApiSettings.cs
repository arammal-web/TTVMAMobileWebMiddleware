namespace TTVMAMobileWebMiddleware.Infrastructure.Settings;

/// <summary>
/// Settings for external API integration
/// </summary>
public sealed class ExternalApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}

