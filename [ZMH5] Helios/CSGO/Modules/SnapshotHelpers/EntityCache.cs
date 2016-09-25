using _ZMH5__Helios.CSGO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Modules.SnapshotHelpers
{
    public class EntityCache<T> where T : BaseEntity, new()
    {
        #region VARIABLES
        private Dictionary<int, T> entDict;
        #endregion

        #region PROPERTIES
        public T[] Entites { get { return entDict.Values.ToArray(); } }
        #endregion

        #region CONSTRUCTORS
        public EntityCache()
        {
            entDict = new Dictionary<int, T>();
        }
        #endregion

        #region METHODS
        public T this[int id]
        {
            get
            {
                if (entDict.ContainsKey(id))
                    return entDict[id];

                var player = Program.Hack.GetEntityByID<T>(id, false);
                if (player != null) 
                    entDict[id] = player;
                return player;
            }
            set
            {
                entDict[id] = value;
            }
        }
        public void Clear()
        {
            entDict.Clear();
        }
        public bool HasID(int id)
        {
            return entDict.ContainsKey(id);
        }
        public void CopyFrom(EntityCache<T> other)
        {
            foreach (var key in other.entDict.Keys)
                entDict[key] = other.entDict[key];
        }
        #endregion
    }
}
