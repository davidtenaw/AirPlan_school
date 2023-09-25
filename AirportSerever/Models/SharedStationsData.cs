using AirportSerever.BL;
using System.Collections.Concurrent;

namespace AirportSerever.Models
{
    public class SharedStationsData
    {
        public List<Station> BrotherStations { get; }
        public Dictionary<string, int> Planes = new Dictionary<string, int>();
        public SemaphoreSlim SharedSemaphore { get; } = new(1);


        public SharedStationsData()
        {
            BrotherStations = new List<Station>();
        }
    }
}
