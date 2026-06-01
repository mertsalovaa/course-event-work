using CourseEventsApi.Hubs;
using CourseEventsApi.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CourseEventsApi.Services
{
    public class DashboardNotifier
    {
        private readonly IHubContext<DashboardHub> _hub;
        private readonly IEventProcessingService _service;

        public DashboardNotifier(
            IHubContext<DashboardHub> hub,
            IEventProcessingService service)
        {
            _hub = hub;
            _service = service;
        }

        public async Task PushUpdateAsync()
        {
            var stats = await _service.GetDashboardStatsAsync();

            await _hub.Clients.All.SendAsync("dashboard:update", stats);
        }
    }
}
