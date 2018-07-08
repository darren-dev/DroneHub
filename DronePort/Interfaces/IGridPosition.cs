namespace DronePort.Interfaces
{
    public interface IGridPosition
    {
        int X { get; }
        int Y { get; }
        bool Completed { get; }

        void SetComplete();
    }
}
