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

        public void Dropitem(SO_InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation());
        }

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        protected virtual Vector3 GetDropLocation()
        {
            return new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        }

        void SpawnPickup(SO_InventoryItem item, Vector3 spawnLocation)
        {
            var pickup = item.SpawnPickup(spawnLocation);
            _droppedItems.Add(pickup);
        }

        #region SAVING

        [System.Serializable]
        struct DropRecord
        {
            public string ItemID;
            public SerializableVector3 DropLocation;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            
            var dropRecords = new DropRecord[_droppedItems.Count];
            for (int i = 0; i < dropRecords.Length; i++)
            {
                dropRecords[i].ItemID = _droppedItems[i].Item.ItemID;
                dropRecords[i].DropLocation = new SerializableVector3(_droppedItems[i].transform.position);
            }

            return dropRecords;
        }

        void ISaveable.RestoreState(object state)
        {
            var dropRecords = (DropRecord[])state;
            foreach (var item in dropRecords)
            {
                var pickupItem = SO_InventoryItem.GetItemFromID(item.ItemID);
                Vector3 position = item.DropLocation.ToVector();
                SpawnPickup(pickupItem, position);
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

        #endregion
    }
}
