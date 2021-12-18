using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker
{
    /// <summary>
    /// Manager for the different entities in the game, such as the player character and the objects in storage.
    /// </summary>
    internal class EntityManager
    {
        public PlayerCharacter Player { get; private set; }

        public EntityManager(PlayerCharacter player)
        {
            Player = player;
        }

        internal List<IDrawable> GetDrawables()
        {
            throw new NotImplementedException();
        }
    }
}
