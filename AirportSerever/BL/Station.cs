using AirportSerever.Hubs;
using AirportSerever.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace AirportSerever.BL
{
    public class Station : StationModel
    {

        private SemaphoreSlim _sem = new(1);
        private static SemaphoreSlim StartDepartureLock = new(1);
        public SharedStationsData SharedData { get; set; } = new SharedStationsData();




        public Station(int id, int timeInStaition = 1000) : base(id, timeInStaition)
        {
        }

        public override async Task<bool> Enter(string name, CancellationTokenSource token)
        {
            if ((Id == 6 || Id == 7) && name.StartsWith("Dep"))
            {
                StartDepartureLock.Wait();
            }

            await _sem.WaitAsync();

            await SharedData.SharedSemaphore.WaitAsync();


            if(token != null)
            {
                CancellationToken t = token.Token;

                try
                {
                    t.ThrowIfCancellationRequested();
                    token.Cancel();
                }
                catch (Exception ex)
                {
                    SharedData.SharedSemaphore.Release();
                    _sem.Release();
                    return false;
                }
                
            }

            if (Plane != null)
                Console.WriteLine("Crash !!!!!!!!");
            Console.WriteLine($"Plane {name}, inside location {Id}");
            Plane = name;
            SharedData.SharedSemaphore.Release();
            return true;
        }

        public override void Exit()
        {
            if ((Id == 6 || Id == 7) && Plane!.StartsWith("Dep"))
            {
                StartDepartureLock.Release();
            }
            Plane = null;
            _sem.Release();

        }


    }
}