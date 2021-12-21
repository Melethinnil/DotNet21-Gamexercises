using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Models;

namespace WarehouseWorker.Managers
{
    /// <summary>
    /// Handles drawing to the console window.
    /// </summary>
    internal interface IScreenManager
    {
        IScreen CurrentScreen { get; }

        void Draw();
    }

    internal class ScreenManager : IScreenManager
    {
        public IScreen CurrentScreen { get; private set; }

        public ScreenManager(IScreen screen)
        {
            CurrentScreen = screen;
        }
        public void Draw()
        {
            CurrentScreen.Draw();
        }
        public void ChangeScreen(IScreen screen)
        {
            if (screen != null)
                CurrentScreen = screen;
        }
    }
}
