using System;
using System.Collections.ObjectModel;
using DronePort.Interfaces;

namespace DronePort.ViewModels
{
    /// <summary>
    /// For two-way binding
    /// </summary>
    [Serializable]
    public class MainWindowViewModel
    {
        private string _title;

        public string PortName { get; set; } = "Amazing Port";
        public int GridHeight { get; set; } = 20;
        public int GridWidth { get; set; } = 20;
        public int StartX { get; set; } = 3;
        public int StartY { get; set; } = 10;

        public ObservableCollection<IOrder> Orders { get; set; }

        public IOrder SelectedOrder { get; set; }

        public MainWindowViewModel()
        {
            Orders = new ObservableCollection<IOrder>();
        }

        public string GetTitle()
        {
            return $"{PortName} ({GridWidth}x{GridHeight}) [{StartX}:{StartY}]";
        }
    }
}

