using System.Linq;
using DronePort.DataPersistence;
using DronePort.Enums;
using DronePort.Interfaces;
using DronePort.Singletons;

namespace DronePort.Logic
{
    public class Port : IPort
    {
        private readonly Storage<IOrder> _orders;
        private readonly Storage<IDroneWithPosition> _drones;

        public IGridMap Map { get; }

        public string Name { get; }
        public int PositionY { get; }
        public int PositionX { get; }

        public int OrderListSize => _orders.Size;
        public int DroneListSize => _drones.Size;

        public Port(string name, IGridMap currentMap, int positionX, int positionY)
        {
            Name = name;
            PositionX = positionX;
            PositionY = positionY;
            Map = currentMap;

            _orders = new Storage<IOrder>();
            _drones = new Storage<IDroneWithPosition>();
        }

        /// <summary>
        /// Calls the display object to update the display of the grid for new positions
        /// </summary>
        public void UpdateDisplay()
        {
            Communicaiton.Instance.GetDisplayObjectAt(PositionX, PositionY).AsPort();
        }

        /// <summary>
        /// Adds a order to the port
        /// </summary>
        /// <param name="order"></param>
        /// <returns>id of order</returns>
        public int AddOrder(IOrder order)
        {
            var insertedOrder = _orders.Add(order);
            Communicaiton.Instance.AddOrderForReference(insertedOrder);
            
            Communicaiton.Instance.GetDisplayObjectAt(order.TargetX, order.TargetY).AsClient();

            var availableDrone = GetAvailableDrone();
            if (availableDrone != null)
            {
                availableDrone.AddOrder(order);
                availableDrone.GoToClient();
            }


            return insertedOrder.Id;
        }

        /// <summary>
        /// Adds a drone to the port - starts working immediately
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>id of drone</returns>
        public int AddDrone(IDrone drone)
        {
            drone.SetPort(this);
            var displayObject = Communicaiton.Instance.GetDisplayObjectAt(drone.CurrentX, drone.CurrentY);
            var insertedDrone = _drones.Add(new DroneWithPosition(drone, displayObject));

            var waitingOrder = GetWaitingOrder();
            if (waitingOrder != null)
            {
                drone.AddOrder(waitingOrder);
            }
            

            return insertedDrone.Id;
        }

        /// <summary>
        /// Completes an order by order id
        /// </summary>
        /// <param name="orderId"></param>
        public void CompleteOrder(int orderId)
        {
            var order = _orders.Query(orderId);
            order.SetStatus(OrderStatus.Completed);
        }

        /// <summary>
        /// Gets an order by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IOrder GetOrderById(int orderId)
        {
            return _orders.Query(orderId);
        }

        /// <summary>
        /// Gets the most optimal path of grid positions for the drone based on the target positionX and positionX.
        /// Automatically sets the drones path and target locations
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns>path list</returns>
        public IGridPosition[] GetPathForDrone(IDrone drone, int targetX, int targetY)
        {
            var path = Map.GetPath(drone.CurrentX, drone.CurrentY, targetX, targetY);

            drone.SetTargetPosition(targetX, targetY);
            drone.SetPath(path);

            return path;
        }

        /// <summary>
        /// Called when a drone has docks, will check for any waiting packages and send the drone out
        /// </summary>
        /// <param name="droneId"></param>
        public void DroneDocked(int droneId)
        {
            var waitingOrder = GetWaitingOrder();
            var availableDrone = _drones.Query(droneId).Drone;

            if (waitingOrder != null)
            {
                availableDrone?.AddOrder(waitingOrder);
            }
        }

        /// <summary>
        /// Gets a path list a drone can use from the current position back to this port
        /// </summary>
        /// <param name="currentDronePosition"></param>
        /// <returns></returns>
        public IGridPosition[] GetPathToPort(IGridPosition currentDronePosition)
        {
            return Map.GetPath(currentDronePosition.X, currentDronePosition.Y, PositionX, PositionY);
        }

        /// <summary>
        /// Updates the display object, effectively moving the visual display of the drone with the current id
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="currentPosition"></param>
        public void UpdateDroneWithNewPosition(int droneId, IGridPosition currentPosition)
        {
            var droneWithPosition = _drones.Query(droneId);
            droneWithPosition.Current.Reset();
            var displayObject = Communicaiton.Instance.GetDisplayObjectAt(currentPosition.X, currentPosition.Y);
            displayObject.AsDrone();
            droneWithPosition.Current = displayObject;
        }

        /// <summary>
        /// Get an order waiting to be collected
        /// </summary>
        /// <returns></returns>
        private IOrder GetWaitingOrder()
        {
            return _orders.All().FirstOrDefault(x => x.Status == OrderStatus.Idle);
        }

        /// <summary>
        /// Returns a drone that's waiting at the warehouse
        /// </summary>
        /// <returns></returns>
        private IDrone GetAvailableDrone()
        {
            foreach (var droneWithPosition in _drones.All())
            {
                if (droneWithPosition.Drone.CurrentState == DroneState.ReadyToCollect)
                {
                    return droneWithPosition.Drone;
                }
            }

            return null;
        }
    }
}