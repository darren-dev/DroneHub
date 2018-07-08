namespace DronePort.Interfaces
{
    public interface IGridMap
    {
        IGridPosition[] GetPath(int currentX, int currentY, int targetX, int targetY);
    }
}