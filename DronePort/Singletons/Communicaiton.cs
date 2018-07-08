using System;
using DronePort.DataType;
using DronePort.Interfaces;
using DronePort.Logic;

namespace DronePort.Singletons
{
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

        public void SetPort(IPort port)
        {
            _currentPort = port;
        }

        public void SetVisualLayer(MainWindow window)
        {
            _mainWindow = window;
        }

        public void CompleteOrder(IOrder order)
        {
            _currentPort.CompleteOrder(order.Id);
        }

        public IGridPosition[] GetPathToPort(IGridPosition currentDronePosition)
        {
            return _currentPort.GetPathToPort(currentDronePosition);
        }

        public VisualGridDisplayObject GetDisplayObjectAt(int x, int y)
        {
            return _mainWindow.GetDisplayObjectAt(x, y);
        }

        public void UpdatePortWithNewPosition(int droneId, IGridPosition currentPosition)
        {
            _currentPort.UpdateDroneWithNewPosition(droneId, currentPosition);
        }

        public IGridPosition[] GetPathForOrder(IDrone drone, IOrder order)
        {
            return _currentPort.GetPathForDrone(drone, order.TargetX,  order.TargetY);
        }

        public void SetDroneDocked(int droneId)
        {
            _currentPort.DroneDocked(droneId);
        }
    }
}