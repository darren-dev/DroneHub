using DronePort.Enums;
using DronePort.Interfaces;
using DronePort.Logic;
using Xunit;

namespace DronePort.Tests
{
    public class PortTests
    {
        private Port _port;
        private GridMap _gridMap;

        public PortTests()
        {
            _gridMap = new GridMap(50, 50);
            _port = new Port("Test Port", _gridMap, 5, 20);
        }

        [Fact]
        public void NewPort_CreatesNewPorts_WithDefaults()
        {
            var currentMap = new GridMap(50, 50);

            var port = new Port("Main Port", currentMap, 5, 5);

            Assert.Equal("Main Port", port.Name);
            Assert.Same(currentMap, port.Map);
            Assert.Equal(5, port.PositionY);
            Assert.Equal(5, port.PositionX);
        }

        [Fact]
        public void AddOrder_ContainsOrder()
        {
            var order = new Order("Darren", "0000000000", 6, 12);
            _port.AddOrder(order);

            Assert.Equal(1, _port.OrderListSize);
        }

        [Fact]
        public void AddDrone_ContainsDrone()
        {
            IDrone drone = new Drone("");
            _port.AddDrone(drone);

            Assert.Equal(1, _port.DroneListSize);
            Assert.Equal("Test Port", drone.PortName);
        }

        [Fact]
        public void Drone_RequestPosition_GetsPosition()
        {
            var drone = new Drone("");

            var pathForDrone = _port.GetPathForDrone(drone, 10, 15);

            Assert.NotEmpty(drone.Path);
            Assert.Same(pathForDrone, drone.Path);
        }

        [Fact]
        public void SendPackage_SetsPackage_SendsOnDrone()
        {
            IOrder order = new Order("Darren", "00000000", 23, 56);
            IDrone drone = new Drone("");
            IPort port = new Port("Management Port", _gridMap, 0, 0);

            port.AddDrone(drone);
            drone.AddOrder(order);
            _port.GetPathForDrone(drone, order.TargetY, order.TargetX);

            bool deliveryValid = drone.IsDeliveryValid();

            drone.GoToClient();

            Assert.True(deliveryValid);
        }

        [Fact]
        public void CompleteOrder_SetsOrderStatusCompleted_AfterQuery()
        {
            IOrder order = new Order("Darren", "00000000", 23, 56);
            IPort port = new Port("Management Port", _gridMap, 0, 0);

            var orderId = port.AddOrder(order);

            port.CompleteOrder(orderId);

            Assert.Equal(OrderStatus.Completed, order.Status);

            IOrder foundOrder = port.GetOrderById(orderId);

            Assert.Equal(OrderStatus.Completed, foundOrder.Status);
        }
    }
}
