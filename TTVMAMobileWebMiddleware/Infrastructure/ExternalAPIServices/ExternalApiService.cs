using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Requests;
using TTVMAMobileWebMiddleware.Domain.Views;
using TTVMAMobileWebMiddleware.Infrastructure.Settings;

namespace TTVMAMobileWebMiddleware.Infrastructure.ExternalAPIServices;

/// <summary>
/// Service implementation for external API communication with basic authentication
/// </summary>
public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _settings;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(
        HttpClient httpClient,
        IOptions<ExternalApiSettings> settings,
        ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        // Configure HttpClient
        var baseUrl = _settings.BaseUrl.TrimEnd('/');
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);

        // Set up basic authentication
        var credentials = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Sends the pending application DTO to external API endpoint
    /// </summary>
    public async Task<ExternalApiResponse> SendPendingApplicationAsync(
        ApprovePendingApplicationDTO approvePendingApplicationDTO,
        CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(approvePendingApplicationDTO, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            // Log the request payload for debugging
            _logger.LogDebug("Sending request to external API. Payload: {Payload}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Use relative endpoint path (BaseAddress is already set)
            var endpoint = "/api/v1.0/IntegrationMiddleware/create-with-child";
            _logger.LogDebug("Calling endpoint: {Endpoint}", endpoint);
            var response = await _httpClient.PostAsync(endpoint, content, ct);

            var responseContent = await response.Content.ReadAsStringAsync(ct);
            var statusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Successfully sent pending application {ApplicationId} to external API. Status: {Status}, Response: {Response}",
                    approvePendingApplicationDTO.Application?.Id, statusCode, responseContent);

                return new ExternalApiResponse
                {
                    IsSuccess = true,
                    StatusCode = statusCode,
                    ResponseContent = responseContent
                };
            }
            else
            {

                var ex = new Exception($"Failed to create application in DLS. Status: {statusCode}, Error: {responseContent}, Request Payload: {json}");
                ex.HelpLink = "failed_application_create";
                throw ex;

            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "HTTP error sending pending application {ApplicationId} to external API",
                approvePendingApplicationDTO.Application?.Id);

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"HTTP Request Exception: {ex.Message}"
            };
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex,
                "Timeout sending pending application {ApplicationId} to external API",
                approvePendingApplicationDTO.Application?.Id);

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"Request Timeout: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error sending pending application {ApplicationId} to external API",
                approvePendingApplicationDTO.Application?.Id);

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"Unexpected Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Creates a new citizen in DLS with address and documents
    /// </summary>
    public async Task<ExternalApiResponse> CreateCitizenWithAddressAndDocumentsAsync(
        CitizenWithDetailsRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            // Log the request payload for debugging
            _logger.LogDebug("Sending create citizen request to external API. Payload: {Payload}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Use relative endpoint path (BaseAddress is already set)
            var endpoint = "/api/v1.0/IntegrationMiddleware/create-with-address-doc-md";
            _logger.LogDebug("Calling endpoint: {Endpoint}", endpoint);
            var response = await _httpClient.PostAsync(endpoint, content, ct);

            var responseContent = await response.Content.ReadAsStringAsync(ct);
            var statusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Successfully created citizen in DLS. Status: {Status}, Response: {Response}",
                    statusCode, responseContent);

                return new ExternalApiResponse
                {
                    IsSuccess = true,
                    StatusCode = statusCode,
                    ResponseContent = responseContent
                };
            }
            else
            {

                var ex = new Exception($"Failed to create citizen in DLS. Status: {statusCode}, Error: {responseContent}, Request Payload: {json}");
                ex.HelpLink = "failed_citizen_create";
                throw ex;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error creating citizen in DLS");

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"HTTP Request Exception: {ex.Message}"
            };
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout creating citizen in DLS");

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"Request Timeout: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating citizen in DLS");

            return new ExternalApiResponse
            {
                IsSuccess = false,
                StatusCode = 0,
                ErrorMessage = $"Unexpected Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Creates a new receipt with details in DLS  
    /// </summary>
    public async Task<ExternalApiResponse> CreateReceiptWithDetailsAsync(
        ReceiptWithDetailRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            // Log the request payload for debugging
            _logger.LogDebug("Sending create receipt request to external API. Payload: {Payload}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Use relative endpoint path (BaseAddress is already set)
            var endpoint = "/api/v1.0/IntegrationMiddleware/create-with-details";
            _logger.LogDebug("Calling endpoint: {Endpoint}", endpoint);
            var response = await _httpClient.PostAsync(endpoint, content, ct);

            var responseContent = await response.Content.ReadAsStringAsync(ct);
            var statusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Successfully created Receipt in DLS. Status: {Status}, Response: {Response}",
                    statusCode, responseContent);

                return new ExternalApiResponse
                {
                    IsSuccess = true,
                    StatusCode = statusCode,
                    ResponseContent = responseContent
                };
            }
            else
            {
                var ex = new Exception($"Failed to create Receipt in DLS. Status: {statusCode}, Error: {responseContent}, Request Payload: {json}");
                ex.HelpLink = "Failed_Receipt_create";
                throw ex;
            }
        }
        catch (HttpRequestException ex)
        {
            var exception = new Exception($"Failed to create Receipt in DLS. HTTP Error: {ex.Message}");
            exception.HelpLink = "failed_receipt_create";
            throw exception;
        }
        catch (TaskCanceledException ex)
        {
            var exception = new Exception($"Failed to create Receipt in DLS. Timeout: {ex.Message}");
            exception.HelpLink = "failed_receipt_create";
            throw exception;
        }
        catch (Exception ex)
        {
            var exception = new Exception($"Failed to create Receipt in DLS. Error: {ex.Message}");
            exception.HelpLink = "failed_receipt_create";
            throw exception;
        }
    }
}

