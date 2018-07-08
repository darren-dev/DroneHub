using DronePort.Logic;
using Xunit;

namespace DronePort.Tests
{
    public class DroneTests
    {
        [Fact]
        public void NewDrone_CreatesNewDrone_WithDefaults()
        {
            var drone = new Drone("");

            Assert.Equal(0, drone.CurrentY);
            Assert.Equal(0, drone.CurrentX);
        }

        [Fact]
        public void SetDroneLocation_SetsCorrectly_FromWarehouse()
        {
            var drone = new Drone("");
            var port = new Port("asd", null, 0, 0);

            drone.SetPort(port);

            Assert.Equal(0, drone.TargetY);
            Assert.Equal(0, drone.TargetX);
        }
    }
}
