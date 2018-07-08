using System;

namespace DronePort.ViewModels
{
    /// <summary>
    /// The object the visual grid references to display each block
    /// </summary>
    public class VisualGridDisplayViewModel
    {
        private readonly Action _onUpdate;
        private bool _port;

        public string VisibleColor { get; set; } = "Red";

        public VisualGridDisplayViewModel(Action onUpdate)
        {
            _port = false;
            _onUpdate = onUpdate;
        }

        public void AsPort()
        {
            _port = true;
            SetVisibleColor("Green");
            callUpdate();
        }

        public void AsClient()
        {
            SetVisibleColor("Blue");
            callUpdate();
        }

        public void AsDrone()
        {
            SetVisibleColor("Purple");
            callUpdate();
        }

        public void Reset()
        {
            SetVisibleColor("Red");
        }

        private void SetVisibleColor(string color)
        {
            VisibleColor = _port ? "Green" : color;
        }

        private void callUpdate()
        {
            _onUpdate();
        }

    }
}