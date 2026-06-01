using CourseEventsApi.Services.Interfaces;

namespace CourseEventsApi.Services
{
    public class ImportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _interval = TimeSpan.FromHours(6); // кожні 6 годин

        public ImportBackgroundService(IServiceProvider services)
            => _services = services;

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var importer = scope.ServiceProvider.GetRequiredService<IEventProcessingService>();
                await importer.ImportAsync();
                await Task.Delay(_interval, ct);
            }
        }
    }
}
