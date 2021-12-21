using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Managers;

namespace WarehouseWorker.Models
{
    /// <summary>
    /// The main screen of the game loop, showing the storage room.
    /// </summary>
    internal class MainGameScreen : IScreen
    {
        private List<IDrawable> _toRedraw = new List<IDrawable>();
        public int RoomSize { get; private set; } = 20;
        public int XMultiplier { get; private set; } = 2;

        public IEntityManager EntityManager { get; private set; }

        public MainGameScreen()
        {
            EntityManager = new EntityManager(this);
            Console.SetWindowSize(50, 50);
            Console.SetBufferSize(50, 50);
            CreateWalls();
        }

        private void CreateWalls()
        {
            //Symbols list: https://en.wikipedia.org/wiki/List_of_Unicode_characters#Block_Elements

            //Horizontal walls
            for(int i = 1; i < RoomSize + 2; i++)
            {
                EntityManager.AddEntity(new Wall('═', i, 0, this));
                EntityManager.AddEntity(new Wall('═', i, RoomSize+2, this));
            }

            //Vertical walls
            for (int i = 1; i < RoomSize + 2; i++)
            {
                EntityManager.AddEntity(new Wall('║', 0, i, this));
                EntityManager.AddEntity(new Wall('║', RoomSize + 2, i, this));
            }

            //Corners
            EntityManager.AddEntity(new Wall('╔', 0, 0, this));
            EntityManager.AddEntity(new Wall('╗', RoomSize + 2, 0, this));
            EntityManager.AddEntity(new Wall('╚', 0, RoomSize + 2, this));
            EntityManager.AddEntity(new Wall('╝', RoomSize + 2, RoomSize + 2, this));
        }

        public void Draw()
        {
            foreach (IDrawable drawable in _toRedraw)
                drawable.Draw();

            _toRedraw.Clear();
        }
        public void MarkForRedraw(IDrawable drawable)
        {
            _toRedraw.Add(drawable);
        }

        public (int X, int Y) GetCursorTarget()
        {
            IPlayerCharacter player = EntityManager.GetPlayer();
            return (player.TargetX, player.TargetY);
        }
    }
}
