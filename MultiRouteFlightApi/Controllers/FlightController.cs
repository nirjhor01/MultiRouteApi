using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiRouteFlightApi.Services;

namespace MultiRouteFlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("routes")]
        public async Task<IActionResult> GetFlightRoutes()
        {
            var routes = await _flightService.GetFlightByRoutesResponsesAsync();
            return Ok(routes);
        }
        [HttpGet("sourceToDestination")]
        public async Task<IActionResult> Routes()
        {

        }


    }
}
