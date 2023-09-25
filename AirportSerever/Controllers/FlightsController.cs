using AirportSerever.BL;
using AirportSerever.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirportSerever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AirportLogic Airport;
        private readonly IHubContext<AirportHub> _airportHub;

        public FlightsController(AirportLogic airportLogic, IHubContext<AirportHub> airportHub)
        {
            Airport = airportLogic;
            _airportHub = airportHub;
        }
     
        // GET api/<FlightsController>/5
        [HttpGet("land/{plane}")]
        public string Land(string plane)
        {
            var msg = $"landing {plane}";
            Console.WriteLine(msg);
            Airport.AddFlight(msg, Direction.Landing);
            return msg;
        }
        [HttpGet("departure/{plane}")]
        public string Departure(string plane)
        {
            var msg = $"Departing {plane}";
            Console.WriteLine(msg);
            Airport.AddFlight(msg, Direction.Departure);
            return msg;
        }
        [HttpGet("status")]
        public Status Status()
        {
            return Airport.GetStatus();
        }
     
    }
}
