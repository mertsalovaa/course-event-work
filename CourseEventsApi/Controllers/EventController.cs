using CourseEventsApi.DTOs;
using CourseEventsApi.Services;
using CourseEventsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;

namespace CourseEventsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IEventProcessingService _ingestionService;
        private readonly DashboardNotifier _dashboardNotifier;

        public EventsController(IEventService service, IEventProcessingService ingestionService, DashboardNotifier dashboardNotifier)
        {
            _service = service;
            _ingestionService = ingestionService;
            _dashboardNotifier = dashboardNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = $"Event {id} not found" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExternalEventDTO dto)
            => Ok(await _service.CreateAsync(dto));

        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            var result = await _ingestionService.ImportAsync();
            return Ok(new
            {
                newImportedCount = result,
                importedAt = DateTime.UtcNow
            });
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _ingestionService.GetDashboardStatsAsync();
            await _dashboardNotifier.PushUpdateAsync();
            return Ok(result);
        }

        [HttpGet("test/disasters")]
        public async Task<IActionResult> TestDisasters()
        {
            var result = await _ingestionService.GetAllAsync();
            return Ok(result);
        }

    }
}
