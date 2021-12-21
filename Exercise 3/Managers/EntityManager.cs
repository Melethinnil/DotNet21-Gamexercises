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
        void MovePlayer(int x, int y, int maxX, int maxY, int minX, int minY);
        void AddEntity(IEntity entity);
        IPlayerCharacter GetPlayer();
        void PickUpOrPutDownAt(int x, int y);
    }

    internal class EntityManager : IEntityManager
    {
        private IPlayerCharacter _player;
        private IScreen _container;
        private List<IEntity> _entities = new List<IEntity>();

        public EntityManager(IScreen container)
        {
            _container = container;
            _player = new PlayerCharacter("Amanda", 'A', ConsoleColor.Green, _container);
            _player.MoveTo(10, 10);
            _player.SetDirection("down");
            AddEntity(new StorageItem(1, "TestItem", "TestCategory", "TestDescription", 10, '#', ConsoleColor.Blue, _container, 5, 5));
        }

        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            _container.MarkForRedraw(entity);
        }

        public IPlayerCharacter GetPlayer()
        {
            return _player;
        }

        public void MovePlayer(int x, int y, int maxX, int maxY, int minX, int minY)
        {
            //Update the player's direction
            string direction;
            if (x < 0)
                direction = "left";
            else if (x > 0)
                direction = "right";
            else if (y < 0)
                direction = "up";
            else
                direction = "down";

            //Move the player if the target square is unoccupied
            bool collision = false;
            int newX = _player.X + x;
            int newY = _player.Y + y;
            //Check if any entity occupies the position you try to move to
            if(_entities.Where(e => e.X == newX && e.Y == newY).Count() > 0)
                collision = true;

            if (!collision)
            {
                _player.MoveTo(newX, newY);
            }

            _player.SetDirection(direction);
        }

        public void PickUpOrPutDownAt(int x, int y)
        {
            IEntity? item;
            if (_player.HeldItem != null)
            {
                item = _player.PutDownItem(x, y);
                if(item != null)
                    _entities.Add(item);
            }
            else
            {
                item = _entities.Find(e => e.X == x && e.Y == y);
                if (item != null)
                {
                    if (item is ICarryable)
                    {
                        _player.PickUpItem((ICarryable)item);
                        _entities.Remove(item);
                    }
                } 
            }
        }
    }
}
