using RPG.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// Stores items equipped to a player. Items are stored by
    /// their equip locations. Place on the player character.
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable
    {
        public event Action OnEquipmentUpdated;

        Dictionary<EquipLocation, SO_EquippableItem> _equippedItems = new Dictionary<EquipLocation, SO_EquippableItem>();

        internal SO_InventoryItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation)) return null;

            return _equippedItems[equipLocation];
        }

        internal void AddItem(EquipLocation equipLocation, SO_EquippableItem item)
        {
            Debug.Assert(item.AllowedEquipLocation == equipLocation);

            _equippedItems[equipLocation] = item;
            OnEquipmentUpdated?.Invoke();
        }

        internal void RemoveItem(EquipLocation equipLocation)
        {
            _equippedItems.Remove(equipLocation);
            OnEquipmentUpdated?.Invoke();
        }

        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();

            foreach (var pair in _equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;  
            }

            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            _equippedItems = new Dictionary<EquipLocation, SO_EquippableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (SO_EquippableItem)SO_InventoryItem.GetItemFromID(pair.Value);

                if (item != null)
                    _equippedItems[pair.Key] = item;
            }
        }
    }
}
