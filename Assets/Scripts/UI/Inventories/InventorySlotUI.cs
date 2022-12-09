using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<SO_InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon = null;

        int _index;
        Inventory _inventory;

        public void Setup(Inventory inventory, int index)
        {
            _inventory = inventory;
            _index = index;
            _icon.SetItem(_inventory.GetItemInSlot(_index));
        }

        public void AddItems(SO_InventoryItem item, int number)
        {
            _inventory.AddToSlot(_index, item);
        }

        public SO_InventoryItem GetItem()
        {
            return _inventory.GetItemInSlot(_index);
        }

        public int GetNumber()
        {
            return 1;
        }

        public int MaxAcceptable(SO_InventoryItem item)
        {
            if (_inventory.HasSpaceFor(item))
                return int.MaxValue;

            return 0;
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(_index);
        }

    }
}
