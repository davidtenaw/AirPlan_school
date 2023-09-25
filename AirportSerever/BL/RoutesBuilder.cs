using AirportSerever.Models;
using System.Data.SqlTypes;

namespace AirportSerever.BL
{
    public class RoutesBuilder
    {
        
        Station[] s = new Station[10];

        public RoutesBuilder()
        {
            for (int i = 0; i < 10; i++)
            {
                s[i] = new Station(i);
            }

            SharedStationsData sharedData = new SharedStationsData();
            sharedData.BrotherStations.Add(s[6]);
            sharedData.BrotherStations.Add(s[7]);
            s[6].SharedData = sharedData;
            s[7].SharedData = sharedData;
        }


        public FlightRoute GetRoute(Direction direction)
        {
            switch (direction)
            {
                case Direction.Landing:
                    {
                        var route = new FlightRoute(LandingRoute());
                        return route;
                    }

                case Direction.Departure:
                    {
                        var route = new FlightRoute(DepartureRoute());
                        return route;
                    }

                default: throw new Exception();
            }
        }

        private Graph LandingRoute()
        {
            var g = new Graph();
            g.AddEdge(s[0], s[1]);
            g.AddEdge(s[1], s[2]);
            g.AddEdge(s[2], s[3]);
            g.AddEdge(s[3], s[4]);
            g.AddEdge(s[4], s[5]);
            g.AddEdge(s[5], s[6]);
            g.AddEdge(s[5], s[7]);
            return g;
        }

        private Graph DepartureRoute()
        {
            var g = new Graph();
            g.AddEdge(s[0], s[6]);
            g.AddEdge(s[0], s[7]);
            g.AddEdge(s[6], s[8]);
            g.AddEdge(s[7], s[8]);
            g.AddEdge(s[8], s[4]);
            g.AddEdge(s[4], s[9]);

            return g;
        }
    }
}