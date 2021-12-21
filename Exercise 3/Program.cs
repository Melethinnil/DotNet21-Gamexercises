using System.Runtime.InteropServices;
using WarehouseWorker.Managers;
using WarehouseWorker.Models;

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetConsoleOutputCP(uint wCodePageID);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetConsoleCP(uint wCodePageID);

SetConsoleOutputCP(65001);
SetConsoleCP(65001);

//Console.CursorVisible = false;

ScreenManager sm = new ScreenManager(new MainGameScreen());
IInputManager im = new InputManager(sm);

while(true)
{
    sm.Draw();
    im.ReadInput();
}