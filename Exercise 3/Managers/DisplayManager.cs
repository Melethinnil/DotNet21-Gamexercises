using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker
{    public struct ScreenSpace
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
    internal class DisplayManager
    {
        private EntityManager _entityManager;
        public ScreenSpace ScreenSize = new ScreenSpace()
        {
            X = 0,
            Y = 0,
            Width = 64,
            Height = 32
        };

        public DisplayManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        private IScreen _currentScreen = new MainScreen();

        public void GoTo(IScreen screen)
        {
            _currentScreen = screen;
        }

        public void Draw()
        {
            Console.Clear();
            //Set the screen size to the desired size
            Console.SetWindowSize(ScreenSize.Width, ScreenSize.Height);
            Console.SetBufferSize(ScreenSize.Width, ScreenSize.Height);

            //Draw the current screen
            _currentScreen.Draw();
            //Draw all non-player entities
            //List<IDrawable> entities = _entityManager.GetDrawables();
            //Draw the player
            _entityManager.Player.Draw();
        }
    }
}
