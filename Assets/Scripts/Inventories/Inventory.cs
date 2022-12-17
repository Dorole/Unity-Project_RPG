using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        public event Action OnInventoryUpdated;

        [SerializeField] SO_InventoryItem[] _items; //serialized for debug purposes
        [SerializeField] int _inventorySize = 16;

        InventorySlot[] _slots;

        public struct InventorySlot
        {
            public SO_InventoryItem Item;
            public int Amount;
        }

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(SO_InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public int GetInventorySize()
        {
            return _slots.Length;
        }

        public bool AddToFirstEmptySlot(SO_InventoryItem item, int amount)
        {
            int i = FindSlot(item);

            if (i < 0)
                return false;

            _slots[i].Item = item;
            _slots[i].Amount += amount;

            OnInventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is an existing stack,
        /// it will add to the stack. Otherwise, it will add to the first empty slot.
        /// </summary>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddToSlot(int slot, SO_InventoryItem item, int amount)
        {
            if (_slots[slot].Item != null)
                return AddToFirstEmptySlot(item, amount);

            int i = FindStack(item);
            if (i >= 0)
                slot = i;

            _slots[slot].Item = item;
            _slots[slot].Amount += amount;
            OnInventoryUpdated?.Invoke();

            return true;
        }

        public bool HasItem(SO_InventoryItem item)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i], item))
                    return true;
            }

            return false;
        }

        public SO_InventoryItem GetItemInSlot(int slot)
        {
            return _slots[slot].Item;
        }

        public int GetItemAmountInSlot(int slot)
        {
            return _slots[slot].Amount;
        }

        public void RemoveFromSlot(int slot, int amount)
        {
            _slots[slot].Amount -= amount;

            if (_slots[slot].Amount <= 0)
            {
                _slots[slot].Amount = 0;
                _slots[slot].Item = null;
            }

            OnInventoryUpdated?.Invoke();
        }

        void Awake()
        {
            _slots = new InventorySlot[_inventorySize];

            //debug purposes
            for (int i = 0; i < _items.Length; i++)
                _slots[i].Item = _items[i];
        }

        int FindSlot(SO_InventoryItem item)
        {
            int i = FindStack(item);

            if (i < 0)
                i = FindEmptySlot();

            return i;
        }

        int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item == null)
                    return i;
            }

            return -1;
        }

        int FindStack(SO_InventoryItem item)
        {
            if (!item.IsStackable) return -1;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].Item, item))
                    return i;
            }

            return -1;
        }


        #region SAVING

        [System.Serializable]
        struct InventorySlotRecord
        {
            public string ItemID;
            public int Amount;
        }

        object ISaveable.CaptureState()
        {
            var slotRecords = new InventorySlotRecord[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].Item != null)
                {
                    slotRecords[i].ItemID = _slots[i].Item.ItemID;
                    slotRecords[i].Amount = _slots[i].Amount;
                }
            }

            return slotRecords;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotRecords= (InventorySlotRecord[])state;

            for (int i = 0; i < _inventorySize; i++)
            {
                _slots[i].Item = SO_InventoryItem.GetItemFromID(slotRecords[i].ItemID);
                _slots[i].Amount = slotRecords[i].Amount;
            }

            OnInventoryUpdated?.Invoke();
        }

        #endregion
    }
}
