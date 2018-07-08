using DronePort.Enums;

namespace DronePort.Interfaces
{
    public interface IDrone : IDataObject
    {
        int CurrentY { get; }
        int CurrentX { get; }
        int TargetY { get; }
        int TargetX { get; }
        bool HasOrder { get; }
        string PortName { get; }

        DroneState CurrentState { get; }
        IOrder CurrentOrder { get; }
        
        void GoToClient();
        void GoToPort();
        void SetTargetPosition(int targetY, int targetX);
        void SetPath(IGridPosition[] path);
        bool SetPort(IPort port);
        void AddOrder(IOrder order);
        bool IsDeliveryValid();
        DroneLocation GetCurrentLocation();
    }
}
