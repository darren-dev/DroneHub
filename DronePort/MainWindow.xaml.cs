using System;
using System.Collections.Generic;
using System.Windows;
using DronePort.DataType;
using DronePort.Logic;
using DronePort.Singletons;

namespace DronePort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowCollection _mainWindowCollection;
        private GridMap _gridMap;
        private Port _port;
        private List<List<VisualGridDisplayObject>> _lstItemsSource;
        private Random _random;

        public MainWindow()
        {
            InitializeComponent();

            _mainWindowCollection = new MainWindowCollection();
            DataContext = _mainWindowCollection;

            _random = new Random();

            Communicaiton.Instance.SetVisualLayer(this);
        }

        /// <summary>
        /// Gets the display object for the grid visualisation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public VisualGridDisplayObject GetDisplayObjectAt(int x, int y)
        {
            return _lstItemsSource[y][x];
        }

        /// <summary>
        /// Creates a new port
        /// </summary>
        private void CreatePort()
        {
            _gridMap = new GridMap(_mainWindowCollection.GridWidth, _mainWindowCollection.GridHeight);
            _port = new Port(_mainWindowCollection.PortName, _gridMap, _mainWindowCollection.StartX, _mainWindowCollection.StartY);
            
            Communicaiton.Instance.SetPort(_port);
        }

        /// <summary>
        /// Creates a new visual grid layer
        /// </summary>
        private void CreateGrid()
        {
            _lstItemsSource = new List<List<VisualGridDisplayObject>>();

            for (int i = 0; i < _mainWindowCollection.GridHeight; i++)
            {
                _lstItemsSource.Add(new List<VisualGridDisplayObject>());

                for (int j = 0; j < _mainWindowCollection.GridWidth; j++)
                {
                    _lstItemsSource[i].Add(new VisualGridDisplayObject(OnDisplayObjectUpdate));
                }
            }
        }

        /// <summary>
        /// Due to the UI being on a separate thread, I need to invoke the refresh method
        /// </summary>
        private void OnDisplayObjectUpdate()
        {
            Dispatcher.Invoke(() => { lst.Items.Refresh(); });
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
            lblTitle.Content = _mainWindowCollection.GetTitle();
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
            var order = new Order("", "", _random.Next(0, _mainWindowCollection.GridWidth), _random.Next(0, _mainWindowCollection.GridHeight));

            _port.AddOrder(order);
        }
    }
}
