using WarehouseWorker;

PlayerCharacter player = new PlayerCharacter("Amanda", 'A', ConsoleColor.Green);
InputHelper playerController = new InputHelper(player);
Console.CursorVisible = false;

while(true)
{
    //Console.Clear();
    player.Draw();
    playerController.ReadKeyInput(Console.ReadKey(true).Key);
}