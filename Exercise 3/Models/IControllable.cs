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

    public struct ScreenPosition
    {
        public int X = 0;
        public int Y = 0;
    }

    internal interface IControllable
    {
        public ScreenPosition Position { get; }
        public void Move(Direction direction);
        public void PickUp(ICarryable item);
        public ICarryable PutDown();
    }
}