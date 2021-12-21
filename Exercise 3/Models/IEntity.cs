namespace WarehouseWorker.Models
{
    internal interface IEntity : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public void MoveTo(int x, int y);
    }
}