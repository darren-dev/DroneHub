using System;

namespace DronePort
{
    /// <summary>
    /// For two-way binding
    /// </summary>
    [Serializable]
    public class MainWindowCollection
    {
        private string _title;

        public string PortName { get; set; } = "Amazing Port";
        public int GridHeight { get; set; } = 20;
        public int GridWidth { get; set; } = 20;
        public int StartX { get; set; } = 3;
        public int StartY { get; set; } = 10;

        public string GetTitle()
        {
            return $"{PortName} ({GridWidth}x{GridHeight}) [{StartX}:{StartY}]";
        }
    }
}

