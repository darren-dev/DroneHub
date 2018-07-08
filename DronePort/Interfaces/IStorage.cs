using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePort.Interfaces
{
    public interface IStorage<T>
    {
        int UsableId { get; }

        int Size { get; }

        T Add(T order);

        bool Remove(int id);
       
        T Query(int id);
    }
}
