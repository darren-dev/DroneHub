using DronePort.Enums;

namespace DronePort.Interfaces
{
    public interface IOrder : IDataObject
    {
        int TargetY { get; }
        int TargetX { get; }
        OrderStatus Status { get; }
        
        void SetLocation(OrderLocation orderLocation);
        void SetStatus(OrderStatus newStatus);
    }
}

