using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Models;

namespace WarehouseWorker.Managers
{
    /// <summary>
    /// Handles the various entities in the game, such as the player and objects in storage.
    /// </summary>
    internal interface IEntityManager
    {
        void MovePlayer(int x, int y, int maxX, int maxY, int minX = 0, int minY = 0);
        IEnumerable<IDrawable> GetDrawables();
    }

    internal class EntityManager : IEntityManager
    {
        private IPlayerCharacter _player;
        private IScreen _container;
        private List<IEntity> _storageItems = new List<IEntity>();

        public EntityManager(IScreen container)
        {
            _container = container;
            _player = new PlayerCharacter("Amanda", "A", ConsoleColor.Green, _container);
            _player.MoveTo(0, 0);
        }

        public IEnumerable<IDrawable> GetDrawables()
        {
            List<IDrawable> drawables = new List<IDrawable>();
            drawables.Add(_player);
            return drawables;
        }

        public void MovePlayer(int x, int y, int maxX, int maxY, int minX = 4, int minY = 4)
        {
            bool collision = false;
            int newX = _player.X + x;
            int newY = _player.Y + y;
            //Prevent moving into walls
            if(newX < minX || newX > maxX || newY < minY || newY > maxY)
                collision = true;

            if (!collision)
            {
                _player.MoveTo(newX, newY); 
            }
        }
    }
}
