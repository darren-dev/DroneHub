namespace DronePort.Interfaces
{
    public interface IPort
    {
        string Name { get; }
        int PositionY { get; }
        int PositionX { get; }
        int AddOrder(IOrder order);
        int AddDrone(IDrone drone);
        void CompleteOrder(int orderId);
        IOrder GetOrderById(int orderId);

        /// <summary>
        /// Gets the path back to the port from the current drone position
        /// </summary>
        /// <param name="currentDronePosition"></param>
        /// <returns>Path to follow</returns>
        IGridPosition[] GetPathToPort(IGridPosition currentDronePosition);

        IGridPosition[] GetPathForDrone(IDrone drone, int targetX, int targetY);

        /// <summary>
        /// Called when a drone docks
        /// </summary>
        void DroneDocked(int droneId);

        void UpdateDroneWithNewPosition(int droneId, IGridPosition currentPosition);
    }
}
