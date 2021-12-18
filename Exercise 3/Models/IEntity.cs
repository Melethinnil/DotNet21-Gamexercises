namespace WarehouseWorker
{
    internal interface IEntity
    {
        public char Symbol { get; }

        public ScreenSpace Position { get; }
        public ConsoleColor Color { get; set; }
    }
}