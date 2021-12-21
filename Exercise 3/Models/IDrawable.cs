namespace WarehouseWorker.Models
{
    /// <summary>
    /// An object that can be drawn in the console as a single character.
    /// </summary>
    internal interface IDrawable
    {
        string Symbol { get; }
        ConsoleColor Color { get; }
        IScreen ContainerScreen { get; }

        void Draw(int xOffset, int yOffset);
        void Undraw();
    }
}