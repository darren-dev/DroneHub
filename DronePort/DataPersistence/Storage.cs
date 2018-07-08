using System.Collections.Generic;
using System.Linq;
using DronePort.Interfaces;

namespace DronePort.DataPersistence
{
    public sealed class Storage<T> : IStorage<T> where T : IDataObject
    {
        private int _currentUsedId;
        private readonly IDictionary<int, T> _inMemoryPersistance = new Dictionary<int, T>();

        public int UsableId => _currentUsedId + 1;
        public int Size => _inMemoryPersistance.Count;

        /// <summary>
        /// Adds an object of type <see cref="T"/>> to the database and returns the objects id
        /// </summary>
        /// <param name="insertObject">object to store</param>
        /// <returns>object with new ID</returns>
        public T Add(T insertObject)
        {
            var usableId = IncrementId(); // Databases usually start at 1
            _inMemoryPersistance.Add(usableId, insertObject);
            insertObject.SetId(usableId);

            return insertObject;
        }

        /// <summary>
        /// Removes an object from the database by id
        /// </summary>
        /// <param name="id">id of the object</param>
        /// <returns>true if the object was removed, false if not</returns>
        public bool Remove(int id)
        {
            return _inMemoryPersistance.ContainsKey(id) && _inMemoryPersistance.Remove(id);
        }

        /// <summary>
        /// Cleares the database of all objects
        /// </summary>
        public void Clear()
        {
            _currentUsedId = 0;
            _inMemoryPersistance.Clear();
        }

        /// <summary>
        /// Retrieve an order by id
        /// </summary>
        /// <param name="id">id of the object</param>
        /// <returns>The object stored, or null</returns>
        public T Query(int id)
        {
            return _inMemoryPersistance.ContainsKey(id) ? _inMemoryPersistance[id] : default(T);
        }

        /// <summary>
        /// Get all the objects in storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> All()
        {
            return _inMemoryPersistance.Select(x => x.Value);
        }

        /// <summary>
        /// Increment the ID and return the new one
        /// </summary>
        /// <returns></returns>
        private int IncrementId()
        {
            _currentUsedId++;
            return _currentUsedId;
        }
    }
}