using DronePort.DataType;

namespace DronePort.Interfaces
{
    public interface IDroneWithPosition : IDataObject
    {
        IDrone Drone { get; set; }
        VisualGridDisplayObject Current { get; set; }
    }

    class DroneWithPosition : IDroneWithPosition
    {
        public int Id { get; private set; }
        public IDrone Drone { get; set; }
        public VisualGridDisplayObject Current { get; set; }

        public DroneWithPosition(IDrone drone, VisualGridDisplayObject current)
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
