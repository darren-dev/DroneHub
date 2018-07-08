using DronePort.Logic;
using Xunit;

namespace DronePort.Tests
{
    public class GridMapTests
    {
        [Fact]
        public void GetPath_Returns_NewPathList()
        {
            var gridMap = new GridMap(200, 200);

            var paths = gridMap.GetPath(10, 10, 50, 185);

            Assert.NotNull(paths);
            Assert.Equal(176, paths.Length);
        }
    }
}
