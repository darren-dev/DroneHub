using DronePort.Enums;
using DronePort.Logic;
using Xunit;

namespace DronePort.Tests
{
    public class OrderTests
    {
        [Fact]
        public void NewOrder_CreatesNewOrder_WithDefaults()
        {
            var order = new Order("Darren", "0000000000", 10, 10);


            Assert.Equal(OrderStatus.Idle, order.Status);
            Assert.Equal(OrderLocation.Warehouse, order.Location);
        }
        
    }
}
