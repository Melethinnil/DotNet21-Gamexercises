using System.Numerics;

namespace WarehouseWorker
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    internal interface IControllable
    {
        public ScreenSpace Position { get; }
        public void Move(Direction direction);
        public void MoveTo(int x, int y);
        public void PickUp(ICarryable item);
        public ICarryable PutDown();
    }
}