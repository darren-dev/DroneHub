using DronePort.Interfaces;

namespace DronePort.Logic
{
    class GridPosition : IGridPosition
    {
        public int X { get; }
        public int Y { get; }
        public bool Completed { get; private set; }

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;

            Completed = false;
        }

        public void SetComplete()
        {
            Completed = true;
        }
    }
}