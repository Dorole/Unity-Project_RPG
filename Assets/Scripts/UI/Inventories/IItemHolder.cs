using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public interface IItemHolder
    {
        SO_InventoryItem GetItem();
    }
}
