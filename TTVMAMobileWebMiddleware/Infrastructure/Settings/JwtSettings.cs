
public sealed class JwtSettings
{
    public string Issuer { get; set; } = "TTVMA";
    public string Audience { get; set; } = "TTVMA-Mobile";
    public string SigningKey { get; set; } = ""; // long random secret
    public int AccessTokenMinutes { get; set; } = 30;
    public int RefreshTokenDays { get; set; } = 30;
}
