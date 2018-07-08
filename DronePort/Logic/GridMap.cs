using DronePort.Interfaces;
using RoyT.AStar;

namespace DronePort.Logic
{
    public class GridMap : IGridMap
    {
        private readonly Grid _grid;

        public GridMap(int width, int height)
        {
            _grid = new Grid(width, height);
        }

        /// <summary>
        /// Get a path list from one position to another
        /// </summary>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        public IGridPosition[] GetPath(int currentX, int currentY, int targetX , int targetY)
        {
            // This is the issue with external libraries not using interfaces
            var positionList = _grid.GetPath(new Position(currentX, currentY),
                new Position(targetX, targetY), MovementPatterns.Full);

            var wantedList = new IGridPosition[positionList.Length];

            for (var i = 0; i < positionList.Length; i++)
            {
                wantedList[i] = new GridPosition(positionList[i].X, positionList[i].Y);
            }

            return wantedList;
        }
    }
}