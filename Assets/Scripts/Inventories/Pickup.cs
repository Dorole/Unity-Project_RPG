using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// Place at the root of a Pickup prefab. Contains data about the pickup.
    /// </summary>
    public class Pickup : MonoBehaviour, ICollectable
    {
        SO_InventoryItem _item;
        int _amount;
        Inventory _inventory;

        public SO_InventoryItem Item => _item;
        public int Amount => _amount;

        void Awake()
        {
            _inventory = Inventory.GetPlayerInventory();
        }

        public void Setup(SO_InventoryItem item, int number)
        {
            _item = item;
            if (!_item.IsStackable) number = 1;
            _amount = number;
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }

        public void HandleCollection()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item, _amount);

            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
        public Transform GetTransform()
        {
            return gameObject.transform;
        }
    }
}
