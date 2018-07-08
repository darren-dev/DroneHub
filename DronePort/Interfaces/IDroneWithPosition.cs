using DronePort.ViewModels;

namespace DronePort.Interfaces
{
    public interface IDroneWithPosition : IDataObject
    {
        IDrone Drone { get; set; }
        VisualGridDisplayViewModel Current { get; set; }
    }

    class DroneWithPosition : IDroneWithPosition
    {
        public int Id { get; private set; }
        public IDrone Drone { get; set; }
        public VisualGridDisplayViewModel Current { get; set; }

        public DroneWithPosition(IDrone drone, VisualGridDisplayViewModel current)
        {
            Drone = drone;
            Current = current;
        }

        public void SetId(int id)
        {
            Id = id;
            Drone.SetId(id);
        }

        
    }
}
