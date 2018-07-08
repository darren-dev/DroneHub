using DronePort.DataPersistence;
using DronePort.Interfaces;
using Xunit;

namespace DronePort.Tests
{
    public class StorageTests
    {
        [Fact]
        public void NewDroneStorage_CreatesNewStorage_WithDefaults()
        {
            var storage = new Storage<IDrone>();

            Assert.Equal(0, storage.Size);
            Assert.Equal(1, storage.UsableId);
        }

        [Fact]
        public void NewOrderStorage_CreatesNewStorage_WithDefaults()
        {
            var storage = new Storage<IOrder>();

            Assert.Equal(0, storage.Size);
            Assert.Equal(1, storage.UsableId);
        }

    }
}
