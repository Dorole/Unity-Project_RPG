namespace RPG.Core.UI.Dragging
{
    /// <summary>
    /// Implementations of this interface can be a destination for
    /// dragging a DragItem.
    /// </summary>
    public interface IDragDestination<T> where T : class
    {
        /// <summary>
        /// How many of the given item can be accepted.
        /// </summary>
        /// <param name="item">The item type to potentially be added.</param>
        /// <returns>If there is no limit Int.MaxValue should be returned.</returns>
        int MaxAcceptable(T item);
        void AddItems(T item, int number);
    }
}