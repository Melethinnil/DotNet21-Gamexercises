namespace WarehouseWorker.Models
{
    internal interface IScreen
    {
        /// <summary>
        /// Shows the screen in the console and returns the screen to go to next.
        /// </summary>
        /// <returns>The screen to switch to.</returns>
        IScreen Show();
        void MarkForRedraw(IDrawable drawable);
    }
}