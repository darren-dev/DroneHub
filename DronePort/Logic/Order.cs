using DronePort.Enums;
using DronePort.Interfaces;

namespace DronePort.Logic
{
    public class Order : IOrder
    {
        private string _clientName;
        private string _clientNumber;
        
        private OrderLocation _orderLocation;

        public int Id { get; private set; }
        public int TargetY { get; }
        public int TargetX { get; }
        public OrderLocation Location { get; }
        public OrderStatus Status { get; private set; }

        public Order(string clientName, string clientNumber, int endX, int endY)
        {
            _clientName = clientName;
            _clientNumber = clientNumber;
            TargetX = endX;
            TargetY = endY;

            Status = OrderStatus.Idle;
            Location = OrderLocation.Warehouse;
        }

        public void SetLocation(OrderLocation orderLocation)
        {
            _orderLocation = orderLocation;
        }

        public void SetStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}