namespace WarehouseWorker.Models
{
    internal interface IScreen
    {
        void Show();
        void MarkForRedraw(IDrawable drawable);
        (int X, int Y) GetCursorTarget();
    }
}