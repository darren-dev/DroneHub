using System.Threading;
using System.Timers;
using DronePort.Enums;
using DronePort.Interfaces;
using DronePort.Singletons;
using Timer = System.Timers.Timer;

namespace DronePort.Logic
{
    public class Drone : IDrone
    {
        private bool _hasOrder;
        private int _targetY;
        private int _targetX;
        private int _currentX;
        private int _currentY;
        private string _name;
        private int _portY;
        private int _portX;
        private int _pathLength;

        private DroneLocation _currentLocation;
        private IOrder _currentOrder;
        private DroneState _currentState;
        private string _portName;

        private bool _canMove;
        private int _gridPositionId;
        private bool _toPort;
        private Timer _timer = null;

        public int CurrentY => _currentY;
        public int CurrentX => _currentX;
        public int TargetY => _targetY;
        public int TargetX => _targetX;
        public string PortName => _portName;

        public int Id { get; private set; }
        public bool HasOrder { get; }
        public DroneState CurrentState => _currentState;
        public IOrder CurrentOrder => _currentOrder;
        public IGridPosition[] Path { get; private set; }
        

        /// <summary>
        /// Creates a new Drone - Implements a new event dispatcher
        /// </summary>
        public Drone(string name)
        {
            _hasOrder = false;
            _name = name;
            _currentState = DroneState.off;
            _gridPositionId = 0;

            NewTimer();
        }

        private void NewTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            _timer = new Timer();
            _timer.Elapsed += OnElapsed;
            _timer.Interval = 200; // In ms
            _timer.Start();
        }


        public bool SetPort(IPort port)
        {
            _portName = port.Name;
            _portX = port.PositionX;
            _portY = port.PositionY;
            _currentX = port.PositionX;
            _currentY = port.PositionY;
            _currentLocation = DroneLocation.Warehouse;
            _currentState = DroneState.ReadyToCollect;

            return true;
        }

        public void GoToClient()
        {
            _toPort = false;
            _canMove = true;
            _currentState = DroneState.toClient;

            NewTimer();
        }

        public void GoToPort()
        {
            _toPort = true;
            GetPathToPort();
            _canMove = true;
            _currentState = DroneState.toWarehouse;

            NewTimer();
        }
        
        /// <summary>
        /// Sets the id of the drone
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        
        /// <summary>
        /// Adds an order to a drone
        /// </summary>
        /// <param name="order"></param>
        public void AddOrder(IOrder order)
        {
            order.SetLocation(OrderLocation.Drone);
            order.SetStatus(OrderStatus.Collected);
            SetPath(Communicaiton.Instance.GetPathForOrder(this, order));
            _currentOrder = order;
            _hasOrder = true;
        }

        // Check if the delivery can be made
        public bool IsDeliveryValid()
        {
            return true;
        }

        public DroneLocation GetCurrentLocation()
        {
            if (_currentY == _portY && _currentX == _portX)
            {
                return DroneLocation.Warehouse;
            }

            if (_currentY == _targetY && _currentX == _targetX)
            {
                return DroneLocation.Client;
            }

            return DroneLocation.EnRoute;
        }
        
        /// <summary>
        /// Sets the target location
        /// </summary>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        public void SetTargetPosition(int targetX, int targetY)
        {
            _targetX = targetX;
            _targetY = targetY;
        }

        /// <summary>
        /// Sets a path list for the drone to use
        /// </summary>
        /// <param name="path"></param>
        public void SetPath(IGridPosition[] path)
        {
            Path = path;
            _pathLength = path.Length;
        }

        /// <summary>
        /// Forcefully overwrites the current location with a new one, bypassing any checks
        /// </summary>
        /// <param name="newLocation"></param>
        public void OverwriteCurrentLocation(DroneLocation newLocation)
        {
            _currentLocation = newLocation;
        }

        /********************************* PRIVATE ***********************/

        private void SetCurrentPosition(int position_id)
        {
            var position = Path[_gridPositionId];
            _currentX = position.X;
            _currentY = position.Y;

            Communicaiton.Instance.UpdatePortWithNewPosition(Id, position);
        }

        private void CompleteOrder()
        {
            Communicaiton.Instance.CompleteOrder(_currentOrder);
            _currentOrder = null;
            _hasOrder = false;
        }

        /// <summary>
        /// Get the path back to port, and send the last grid point as reference
        /// </summary>
        private void GetPathToPort()
        {
            var pathToPort = Communicaiton.Instance.GetPathToPort(Path[Path.Length - 1]);
            SetPath(pathToPort);
        }

        /// <summary>
        /// What to do when the drone has reached the destination
        /// </summary>
        private void ReachedDestination()
        {
            if (_toPort)
            {
                _currentState = DroneState.ReadyToCollect;
                _currentLocation = DroneLocation.Warehouse;
                Communicaiton.Instance.SetDroneDocked(Id);
            }

            if (!_toPort)
            {
                CompleteOrder();
                GoToPort();
            }
        }

        /// <summary>
        /// What to do every second
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnElapsed(object source, ElapsedEventArgs e)
        {
            var s = (Timer) source;
            if (!s.Enabled)
            {
                return;
            }
            if (_canMove)
            {
                Interlocked.Increment(ref _gridPositionId);

                if (_gridPositionId < _pathLength)
                {
                    SetCurrentPosition(_gridPositionId);
                }
                else
                {
                    _canMove = false;
                    _gridPositionId = 0;
                    ReachedDestination();
                }
            }
            else
            {
                _gridPositionId = 0;
                if (_hasOrder)
                {
                    GoToClient();
                }
            }
        }
    }
}
