using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        List<Pickup> _droppedItems = new List<Pickup>();

        public void Dropitem(SO_InventoryItem item, int amount)
        {
            SpawnPickup(item, GetDropLocation(), amount);
        }

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        protected virtual Vector3 GetDropLocation()
        {
            return new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        }

        void SpawnPickup(SO_InventoryItem item, Vector3 spawnLocation, int amount)
        {
            var pickup = item.SpawnPickup(spawnLocation, amount);
            _droppedItems.Add(pickup);
        }

        #region SAVING

        [System.Serializable]
        struct DropRecord
        {
            public string ItemID;
            public SerializableVector3 DropLocation;
            public int Amount;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            
            var dropRecords = new DropRecord[_droppedItems.Count];
            for (int i = 0; i < dropRecords.Length; i++)
            {
                dropRecords[i].ItemID = _droppedItems[i].Item.ItemID;
                dropRecords[i].DropLocation = new SerializableVector3(_droppedItems[i].transform.position);
                dropRecords[i].Amount = _droppedItems[i].Amount;
            }

            return dropRecords;
        }

        void ISaveable.RestoreState(object state)
        {
            ClearDroppedItemsList();

            var dropRecords = (DropRecord[])state;
            foreach (var item in dropRecords)
            {
                var pickupItem = SO_InventoryItem.GetItemFromID(item.ItemID);
                Vector3 position = item.DropLocation.ToVector();
                int amount = item.Amount;

                SpawnPickup(pickupItem, position, amount);
            }
        }

        /// <summary>
        /// Remove drops that have been picked up and update the drop list.
        /// </summary>
        void RemoveDestroyedDrops()
        {
            var uppdatedDroppedItems = new List<Pickup>();

            foreach (var item in _droppedItems)
            {
                if (item != null)
                    uppdatedDroppedItems.Add(item);
            }

            _droppedItems = uppdatedDroppedItems;
        }

        void ClearDroppedItemsList()
        {
            if (_droppedItems.Count != 0)
            {
                foreach (var item in _droppedItems)
                    Destroy(item.gameObject);

                _droppedItems.Clear();
            }
        }

        #endregion
    }
}
