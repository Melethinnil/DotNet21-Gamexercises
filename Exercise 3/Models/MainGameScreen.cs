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
            Console.SetWindowSize(50, 25);
            Console.SetBufferSize(50, 25);
            CreateWalls();
        }

        private void CreateWalls()
        {
            //Symbols list: https://en.wikipedia.org/wiki/List_of_Unicode_characters#Block_Elements

            //Horizontal walls
            _toRedraw.Add(new Wall("╔".PadRight(RoomSize * XMultiplier + 2, '═') + "╗", 0, 0, this, true));
            _toRedraw.Add(new Wall("╚".PadRight(RoomSize * XMultiplier + 2, '═') + "╝", 0, RoomSize + 1, this, true));

            //Vertical walls
            _toRedraw.Add(new Wall("╔".PadRight(RoomSize + 1, '║') + "╚", 0, 0, this));
            _toRedraw.Add(new Wall("╗".PadRight(RoomSize + 1, '║') + "╝", RoomSize * XMultiplier + 2, 0, this));
        }

        public void Draw()
        {
            foreach (IDrawable drawable in _toRedraw)
                drawable.Draw(1, 1);

            _toRedraw.Clear();
        }
        public void MarkForRedraw(IDrawable drawable)
        {
            _toRedraw.Add(drawable);
        }
    }
}
