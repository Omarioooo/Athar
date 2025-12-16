namespace AtharPlatform.Services.MachineLearning
{
    /// <summary>
    /// Background service that periodically trains ML models
    /// Trains models daily or when triggered manually
    /// </summary>
    public class MLModelTrainingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MLModelTrainingBackgroundService> _logger;
        private readonly TimeSpan _trainingInterval = TimeSpan.FromHours(24); // Train daily
        private readonly TimeSpan _initialDelay = TimeSpan.FromMinutes(5); // Wait 5 minutes after startup

        public MLModelTrainingBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<MLModelTrainingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ML Model Training Background Service starting...");

            // Wait before first training to let the application initialize
            await Task.Delay(_initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting scheduled ML model training...");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var recommendationService = scope.ServiceProvider
                            .GetRequiredService<ICampaignRecommendationService>();

                        await recommendationService.TrainModelsAsync();
                    }

                    _logger.LogInformation("Scheduled ML model training completed. Next training in {Hours} hours",
                        _trainingInterval.TotalHours);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during scheduled ML model training");
                }

                // Wait for next training cycle
                await Task.Delay(_trainingInterval, stoppingToken);
            }

            _logger.LogInformation("ML Model Training Background Service stopping...");
        }
    }
}
