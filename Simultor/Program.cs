namespace Simultor
{
    public class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            Console.WriteLine("Hello, World!");
            int timeUntilChek = 1;
            var i = 1;
            while (true)
            {
                try
                {
                    if (Environment.TickCount % 2 == 0)
                        client.GetAsync($"https://localhost:7072/api/Flights/land/f{i++}").Wait();
                    else
                        client.GetAsync($"https://localhost:7072/api/Flights/departure/f{i++}").Wait();
                }
                catch(Exception x)
                {

                    Console.WriteLine($"serever is sleeping, exception:{x.Message}");
                    Thread.Sleep(timeUntilChek++*1000);

                }
                Thread.Sleep(1000);

            }
        }
    }
}