namespace WarehouseWorker.Models
{
    /// <summary>
    /// An object that can be drawn in the console as a single character.
    /// </summary>
    internal interface IDrawable
    {
        public ColoredSymbol Symbol { get; }
        IScreen ContainerScreen { get; }

        void Draw();
        void Undraw();
    }
}