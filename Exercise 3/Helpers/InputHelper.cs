using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker
{
    internal class InputHelper
    {
        private IControllable _player;
        public InputHelper(IControllable player)
        {
            _player = player;
        }
        public void ReadKeyInput(ConsoleKey key)
        {
            switch(key)
            {
                case ConsoleKey.UpArrow:
                    _player.Move(Direction.Up);
                    break;
                case ConsoleKey.RightArrow:
                    _player.Move(Direction.Right);
                    break;
                case ConsoleKey.DownArrow:
                    _player.Move(Direction.Down);
                    break;
                case ConsoleKey.LeftArrow:
                    _player.Move(Direction.Left);
                    break;
            }
        }
    }
}
