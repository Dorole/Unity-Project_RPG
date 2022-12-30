using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        public event Action OnStoreUpdated;

        Dictionary<int, DockedItemSlot> _dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public SO_ActionItem Item;
            public int Number;
        }

        public bool Use(int index, GameObject target)
        {
            if (_dockedItems.ContainsKey(index))
            {
                _dockedItems[index].Item.Use(target);

                if (_dockedItems[index].Item.IsConsumable)
                    RemoveItems(index, 1);

                return true;
            }

            return false;
        }

        internal void AddAction(SO_InventoryItem item, int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {
                if (ReferenceEquals(item, _dockedItems[index].Item))
                    _dockedItems[index].Number += number;
            }

            else
            {
                var slot = new DockedItemSlot();
                slot.Item = item as SO_ActionItem;
                slot.Number = number;
                _dockedItems[index] = slot;
            }

            OnStoreUpdated?.Invoke();
        }

        /// <summary>
        /// Get the number of items left at the given index.
        /// </summary>
        /// <returns>
        /// Returns 0 if no item is in the index or the item has
        /// been fully consumed.
        /// </returns>
        internal int GetNumber(int index)
        {
            if (_dockedItems.ContainsKey(index))
                return _dockedItems[index].Number;

            return 0;
        }

        internal SO_InventoryItem GetAction(int index)
        {
            if (_dockedItems.ContainsKey(index))
                return _dockedItems[index].Item;

            return null;
        }

        /// <summary>
        /// What is the maximum number of items allowed in this slot.
        /// Takes into account whether the slot already contains an item
        /// and whether it is the same type. Will only accept multiple if the
        /// item is consumable.
        /// </summary>
        /// <returns>Will return int.MaxValue when there is no effective bound.</returns>
        internal int MaxAcceptable(SO_InventoryItem item, int index)
        {
            var actionItem = item as SO_ActionItem;
            if (!actionItem) return 0;

            if (_dockedItems.ContainsKey(index) && !ReferenceEquals(item, _dockedItems[index].Item))
                return 0;

            if (actionItem.IsConsumable)
                return int.MaxValue;

            if (_dockedItems.ContainsKey(index))
                return 0;

            return 1;
        }

        internal void RemoveItems(int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {
                _dockedItems[index].Number -= number;

                if (_dockedItems[index].Number <= 0)
                    _dockedItems.Remove(index);

                OnStoreUpdated?.Invoke();
            }
        }

        #region SAVING

        [System.Serializable]
        struct DockedItemsRecord
        {
            public string ItemID;
            public int Number;
        }

        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, DockedItemsRecord>();

            foreach (var pair in _dockedItems)
            {
                var record = new DockedItemsRecord();
                record.ItemID = pair.Value.Item.ItemID;
                record.Number = pair.Value.Number;
                state[pair.Key] = record;
            }

            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var dockedItemsDict = (Dictionary<int, DockedItemsRecord>)state;

            foreach (var pair in dockedItemsDict)
            {
                AddAction(SO_InventoryItem.GetItemFromID(pair.Value.ItemID), pair.Key, pair.Value.Number);
            }
        }

        #endregion
    }
}
