namespace WarehouseWorker.Models
{
    internal interface IScreen
    {
        int XMultiplier { get; }

        void Draw();
        void MarkForRedraw(IDrawable drawable);
    }
}