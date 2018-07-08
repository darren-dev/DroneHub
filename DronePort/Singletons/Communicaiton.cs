using System;
using DronePort.DataType;
using DronePort.Interfaces;

namespace DronePort.Singletons
{
    /// <summary>
    /// The makeshift communication layer between Drones, Ports, and the visual layer
    /// </summary>
    public sealed class Communicaiton
    {
        private static readonly Lazy<Communicaiton> Lazy =
            new Lazy<Communicaiton>(() => new Communicaiton());

        public static Communicaiton Instance => Lazy.Value;

        private IPort _currentPort;
        private MainWindow _mainWindow;

        private Communicaiton()
        {
        }

        /// <summary>
        /// Sets the port for reference
        /// </summary>
        /// <param name="port"></param>
        public void SetPort(IPort port)
        {
            _currentPort = port;
        }

        /// <summary>
        /// Sets the visual layer for reference
        /// </summary>
        /// <param name="window"></param>
        public void SetVisualLayer(MainWindow window)
        {
            _mainWindow = window;
        }

        /// <summary>
        /// Update the current port to complete the set order
        /// </summary>
        /// <param name="order"></param>
        public void CompleteOrder(IOrder order)
        {
            _currentPort.CompleteOrder(order.Id);
        }

        /// <summary>
        /// Gets the path from one location back to the reference port
        /// </summary>
        /// <param name="currentDronePosition"></param>
        /// <returns></returns>
        public IGridPosition[] GetPathToPort(IGridPosition currentDronePosition)
        {
            return _currentPort.GetPathToPort(currentDronePosition);
        }

        /// <summary>
        /// Gets the display object from the reference visual layer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public VisualGridDisplayObject GetDisplayObjectAt(int x, int y)
        {
            return _mainWindow.GetDisplayObjectAt(x, y);
        }

        /// <summary>
        /// Updates the port with the new drone position
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="currentPosition"></param>
        public void UpdatePortWithNewPosition(int droneId, IGridPosition currentPosition)
        {
            _currentPort.UpdateDroneWithNewPosition(droneId, currentPosition);
        }

        /// <summary>
        /// Gets the path from the drone's current position to the order position
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IGridPosition[] GetPathForOrder(IDrone drone, IOrder order)
        {
            return _currentPort.GetPathForDrone(drone, order.TargetX,  order.TargetY);
        }

        /// <summary>
        /// Called when a drone docks in the reference port
        /// </summary>
        /// <param name="droneId"></param>
        public void SetDroneDocked(int droneId)
        {
            _currentPort.DroneDocked(droneId);
        }
    }
}