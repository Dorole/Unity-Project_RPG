using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour
    {
        //serialized for debug purposes
        [SerializeField] SO_InventoryItem[] _items;
        
        public event Action OnInventoryUpdated;

        [SerializeField] int _inventorySize = 16;

        SO_InventoryItem[] _slots;

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

        public bool AddToFirstEmptySlot(SO_InventoryItem item)
        {
            int i = FindSlot(item);

            if (i < 0)
                return false;

            _slots[i] = item;
            OnInventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is an existing stack,
        /// it will add to the stack. Otherwise, it will add to the first empty slot.
        /// </summary>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddToSlot(int slot, SO_InventoryItem item)
        {
            if (_slots[slot] != null)
                return AddToFirstEmptySlot(item);

            _slots[slot] = item;
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
            return _slots[slot];
        }

        public void RemoveFromSlot(int slot)
        {
            _slots[slot] = null;
            OnInventoryUpdated?.Invoke();
        }

        void Awake()
        {
            _slots = new SO_InventoryItem[_inventorySize];

            //debug purposes
            for (int i = 0; i < _items.Length; i++)
                _slots[i] = _items[i];
        }

        int FindSlot(SO_InventoryItem item)
        {
            return FindEmptySlot();
        }

        int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                    return i;
            }

            return -1;
        }

    }
}
