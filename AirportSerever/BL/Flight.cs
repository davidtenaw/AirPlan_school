using AirportSerever.Hubs;
using AirportSerever.Models;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using static System.Collections.Specialized.BitVector32;

namespace AirportSerever.BL
{
    public class Flight : FlightModel
    {

        public Flight(string flightName, FlightRoute route, IHubContext<AirportHub> airportHub) : base(flightName, route, airportHub)
        {
            Run();
        }

        internal override void Run()
        {
            Task.Run(async () =>
            {


                Station? currStation = null;
                var nextStations = _route.GetNextStations(currStation);
                while (nextStations.Count > 0)
                {

                    Task<Station> task = null;

                    if (nextStations.Count == 1)
                        task = MoveToStation(nextStations.First(), currStation!);
                    else if (nextStations.Count > 1)
                    {
                        Station station6 = nextStations.Where(station => station.Id == 6).First();
                        if (station6.Plane == null)
                            task = MoveToStation(station6, currStation!);

                        Station station7 = nextStations.Where(station => station.Id == 7).First();
                        if (task == null && station7.Plane == null)
                            task = MoveToStation(station7, currStation!);

                        if (task == null)
                        {
                            CancellationTokenSource token = new CancellationTokenSource();
                            var tasks = nextStations.Select(async s => await MoveToStation(s, currStation!, token));
                            task = Task.WhenAny(tasks).Result!;

                            if (await task == null) 
                                task = tasks.Where(t => t != task && task.Result != null).First();
                        }
                    }

                    

                    task.ConfigureAwait(false);

                    currStation = await task!;

                    


                    
                    nextStations = _route.GetNextStations(currStation);

                    if ((currStation.Id == 6 || currStation.Id == 7) && Name.StartsWith("land"))// && nextStations.Count == 0)
                        Console.WriteLine($"landing no next station: Plane {Name}, currStation {currStation}");//, nextStation {nextStations.First().Id} ");
                }
                Console.WriteLine($"plane Finish {Name} ");
                currStation!.Exit();
                currStation.Plane = null;
                _ = _airportHub.Clients.All.SendAsync(currStation.Id.ToString(), "");




            });

        }

        internal async override Task<Station?> MoveToStation(Station nextStation, Station prevStation, CancellationTokenSource token = null)
        {
            Task<bool> t = nextStation.Enter(Name, token);
            t.ConfigureAwait(false);
            if (!await t)
            {
                return null;
            }
            if (prevStation != null)
            {
                prevStation.Exit();
                _ = _airportHub.Clients.All.SendAsync(prevStation.Id.ToString(), "");
            }
            Console.WriteLine(value: $"Flight {Name} is at {nextStation.Id}");
            _ = _airportHub.Clients.All.SendAsync(nextStation.Id.ToString(), Name);
            await Task.Delay(nextStation.TimeInStaition);

            return nextStation;
        }


    }
}