using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TTVMAMobileWebMiddleware.Application.Interfaces;

namespace TTVMAMobileWebMiddleware.Application.Workers;

/// <summary>
/// Background worker service that syncs receipts and driving licenses every 20 minutes
/// </summary>
public class ReceiptSyncWorker : BackgroundService
{
    private readonly ILogger<ReceiptSyncWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _period = TimeSpan.FromMinutes(10);
    private readonly int _systemUserId;

    public ReceiptSyncWorker(
        ILogger<ReceiptSyncWorker> logger,
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _systemUserId = configuration.GetValue<int>("SyncSettings:SystemUserId", 1);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ReceiptSyncWorker started. Will run every {Period} minutes", _period.TotalMinutes);

        // Wait a bit before the first run to allow the application to fully start
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        using var periodicTimer = new PeriodicTimer(_period);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("step in");
                try
                {
                    await SyncReceiptsAndDrivingLicensesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during sync execution, will retry on next cycle");
                }

                // Wait for the next tick, but handle cancellation
                try
                {
                    await periodicTimer.WaitForNextTickAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("ReceiptSyncWorker is stopping due to cancellation");
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("ReceiptSyncWorker is stopping due to cancellation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReceiptSyncWorker encountered an error and is stopping");
        }
    }

    private async Task SyncReceiptsAndDrivingLicensesAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting scheduled sync of receipts and driving licenses at {Time}", DateTime.UtcNow);

            using var scope = _serviceScopeFactory.CreateScope();
            var receiptService = scope.ServiceProvider.GetRequiredService<IReceiptService>();

            var result = await receiptService.SyncReceiptsAndDrivingLicensesAsync(_systemUserId, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    "Sync completed successfully. Receipts: {Receipts}, ReceiptDetails: {ReceiptDetails}, " +
                    "DrivingLicenses: {DrivingLicenses}, DrivingLicenseDetails: {DrivingLicenseDetails}, " +
                    "Citizens: {Citizens}, Applications: {Applications}",
                    result.ReceiptsSynced,
                    result.ReceiptDetailsSynced,
                    result.DrivingLicensesSynced,
                    result.DrivingLicenseDetailsSynced,
                    result.CitizensProcessed,
                    result.ApplicationsProcessed);
            }
            else
            {
                _logger.LogWarning(
                    "Sync completed with errors. Receipts: {Receipts}, ReceiptDetails: {ReceiptDetails}, " +
                    "DrivingLicenses: {DrivingLicenses}, DrivingLicenseDetails: {DrivingLicenseDetails}, " +
                    "Errors: {ErrorCount}",
                    result.ReceiptsSynced,
                    result.ReceiptDetailsSynced,
                    result.DrivingLicensesSynced,
                    result.DrivingLicenseDetailsSynced,
                    result.Errors.Count);

                foreach (var error in result.Errors)
                {
                    _logger.LogError("Sync error: {Error}", error);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during scheduled sync of receipts and driving licenses");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ReceiptSyncWorker is stopping");
        await base.StopAsync(cancellationToken);
    }
}

