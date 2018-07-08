using System;
using System.Collections.Generic;
using System.Windows;
using DronePort.Interfaces;
using DronePort.Logic;
using DronePort.Singletons;
using DronePort.ViewModels;

namespace DronePort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private GridMap _gridMap;
        private Port _port;
        private List<List<VisualGridDisplayViewModel>> _lstItemsSource;
        private Random _random;
        private int _temp_number;

        public MainWindow()
        {
            InitializeComponent();

            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;

            _random = new Random();

            Communicaiton.Instance.SetVisualLayer(this);
        }

        /// <summary>
        /// Gets the display object for the grid visualisation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public VisualGridDisplayViewModel GetDisplayObjectAt(int x, int y)
        {
            return _lstItemsSource[y][x];
        }

        /// <summary>
        /// Creates a new port
        /// </summary>
        private void CreatePort()
        {
            _gridMap = new GridMap(_mainWindowViewModel.GridWidth, _mainWindowViewModel.GridHeight);
            _port = new Port(_mainWindowViewModel.PortName, _gridMap, _mainWindowViewModel.StartX, _mainWindowViewModel.StartY);
            
            Communicaiton.Instance.SetPort(_port);
        }

        /// <summary>
        /// Creates a new visual grid layer
        /// </summary>
        private void CreateGrid()
        {
            _lstItemsSource = new List<List<VisualGridDisplayViewModel>>();

            for (int i = 0; i < _mainWindowViewModel.GridHeight; i++)
            {
                _lstItemsSource.Add(new List<VisualGridDisplayViewModel>());

                for (int j = 0; j < _mainWindowViewModel.GridWidth; j++)
                {
                    _lstItemsSource[i].Add(new VisualGridDisplayViewModel(OnDisplayObjectUpdate));
                }
            }
        }

        /// <summary>
        /// Due to the UI being on a separate thread, I need to invoke the refresh method
        /// </summary>
        private void OnDisplayObjectUpdate()
        {
            Dispatcher.Invoke(() =>
            {
                lst.Items.Refresh();

                UpdateOrderDisplayStatus();
            });
        }

        /// <summary>
        /// Updates the display status of the currently selected order
        /// </summary>
        private void UpdateOrderDisplayStatus()
        {
            if (_mainWindowViewModel.SelectedOrder != null)
            {
                var order = _port.GetOrderById(_mainWindowViewModel.SelectedOrder.Id);
                lblClientName.Content = $"Client name: {order.ClientName}";
                lblOrderStatus.Content = $"Status: {order.Status.ToString()}";
                lblTargetLocation.Content = $"X: {order.TargetX} - Y:{order.TargetY}";
            }
        }

        /// <summary>
        /// Binds a new list to the visual list control
        /// </summary>
        private void BindToGridControl()
        {
            lst.Items.Clear();
            lst.ItemsSource = _lstItemsSource;
        }

        /// <summary>
        /// Adds a new order to reference from the front end
        /// </summary>
        /// <param name="order"></param>
        public void AddOrder(IOrder order)
        {
            _mainWindowViewModel.Orders.Add(order);
        }

        /// <summary>
        /// Creates a grid and port based on user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreatePort();
            CreateGrid();
            BindToGridControl();

            grpCreate.Visibility = Visibility.Collapsed;
            lblTitle.Content = _mainWindowViewModel.GetTitle();
            btnAddDrone.IsEnabled = true;
            btnAddOrder.IsEnabled = true;

            _port.UpdateDisplay();
        }

        /// <summary>
        /// Adds a new drone to the port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            _port.AddDrone(new Drone("Droner"));
        }
        
        /// <summary>
        /// Adds a new order at a random location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            _temp_number++;
            var clientName = $"Client_{_temp_number}";
            var clientNumber = $"{_temp_number}{_temp_number}{_temp_number}{_temp_number}{_temp_number}{_temp_number}{_temp_number}";

            var order = new Order(clientName, clientNumber, _random.Next(0, _mainWindowViewModel.GridWidth), _random.Next(0, _mainWindowViewModel.GridHeight));

            _port.AddOrder(order);
        }

        private void cmboOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateOrderDisplayStatus();
        }
    }
}
