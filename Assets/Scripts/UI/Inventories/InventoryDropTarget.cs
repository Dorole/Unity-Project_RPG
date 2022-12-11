using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// Handles spawning pickups when item dropped into the world. 
    /// Must be placed on the root canvas where items can be dragged.
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<SO_InventoryItem>
    {
        public void AddItems(SO_InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().Dropitem(item);
        }

        public int MaxAcceptable(SO_InventoryItem item)
        {
            return int.MaxValue;
        }

    }
}
