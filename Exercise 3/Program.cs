using WarehouseWorker;

PlayerCharacter player = new PlayerCharacter("Amanda", 'A', ConsoleColor.Green);
InputManager playerController = new InputManager(player);
EntityManager entityManager = new EntityManager(player);
DisplayManager displayManager = new DisplayManager(entityManager);
Console.CursorVisible = false;

while(true)
{
    //Console.Clear();
    displayManager.Draw();
    playerController.ReadKeyInput(Console.ReadKey(true).Key);
}