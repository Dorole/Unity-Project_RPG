using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// Place at the root of a Pickup prefab. Contains data about the pickup.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        SO_InventoryItem _item;
        Inventory _inventory;

        public SO_InventoryItem Item => _item;

        void Awake()
        {
            _inventory = Inventory.GetPlayerInventory();
        }

        public void Setup(SO_InventoryItem item)
        {
            _item = item;
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item);

            if (foundSlot)
                Destroy(gameObject);
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }

    }
}
