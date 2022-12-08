namespace RPG.Core.UI.Dragging
{
    /// <summary>
    /// Implementations of this interface can be a source for 
    /// dragging a "DragItem".
    /// </summary>
    /// <typeparam name="T">The type that represents the item being dragged.</typeparam>
    public interface IDragSource<T> where T : class
    {
        T GetItem();
        int GetNumber();

        /// <summary>
        /// Remove a number of items. The number should never exceed the number returned by GetNumber;
        /// </summary>
        void RemoveItems(int number);
    }
}