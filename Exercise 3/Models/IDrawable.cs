namespace WarehouseWorker
{
    internal interface IDrawable
    {
        public char Symbol { get; }
        public ConsoleColor Color { get; set; }
        public void Draw();
        public void UnDraw();
    }
}