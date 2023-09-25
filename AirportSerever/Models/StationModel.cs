namespace AirportSerever.Models
{
    public abstract class StationModel
    {
        public string? Plane = null;

        public int Id { get; }
        public int TimeInStaition { get; internal set; } = 1000;

        public StationModel(int id, int timeInStaition)
        {
            Id = id;
            TimeInStaition = timeInStaition;
        }
        public  abstract  Task<bool> Enter(string name, CancellationTokenSource token);
        public abstract void Exit();
    }
}
