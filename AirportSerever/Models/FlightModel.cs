using AirportSerever.BL;
using AirportSerever.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AirportSerever.Models
{
    public abstract class FlightModel
    {
        public string Name;
        public int StationId = 0;
        public FlightRoute _route;
        public readonly IHubContext<AirportHub> _airportHub;

        public FlightModel(string flightName, FlightRoute route, IHubContext<AirportHub> airportHub)
        {
            Name = flightName;
            _route = route;
            _airportHub = airportHub;
        }
        internal abstract void Run();
        internal abstract  Task<Station?> MoveToStation(Station nextStation, Station prevStation, CancellationTokenSource tokenSource);

    }
}
