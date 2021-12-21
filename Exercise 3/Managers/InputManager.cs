using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Models;

namespace WarehouseWorker.Managers
{
    /// <summary>
    /// Handles all keyboard input.
    /// </summary>
    internal interface IInputManager
    {
        public void ReadInput();
    }

    internal class InputManager : IInputManager
    {
        private IScreenManager _screenManager;

        public InputManager(IScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public void ReadInput()
        {
            IScreen screen = _screenManager.CurrentScreen;


            if (screen is MainGameScreen)
            {
                MainGameScreen scr = (MainGameScreen)screen;
                IEntityManager em = scr.EntityManager;
                ConsoleKey key = Console.ReadKey(true).Key;
                int x = 0;
                int y = 0;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        y = -1;
                        em.MovePlayer(x, y, scr.RoomSize, scr.RoomSize);
                        break;
                    case ConsoleKey.DownArrow:
                        y = 1;
                        em.MovePlayer(x, y, scr.RoomSize, scr.RoomSize-1);
                        break;
                    case ConsoleKey.LeftArrow:
                        x = -1;
                        em.MovePlayer(x, y, scr.RoomSize, scr.RoomSize);
                        break;
                    case ConsoleKey.RightArrow:
                        x = 1;
                        em.MovePlayer(x, y, scr.RoomSize, scr.RoomSize);
                        break;
                } 
            }
        }
    }
}
