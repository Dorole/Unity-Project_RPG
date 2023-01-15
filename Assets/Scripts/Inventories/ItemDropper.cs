using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        List<Pickup> _droppedItems = new List<Pickup>();
        List<DropRecord> _itemsDroppedInOtherScenes = new List<DropRecord>();

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
            public int sceneBuildIndex;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();

            var droppedItemsList = new List<DropRecord>();
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            foreach(Pickup pickup in _droppedItems)
            {
                var droppedItem = new DropRecord();
                droppedItem.ItemID = pickup.Item.ItemID;
                droppedItem.DropLocation = new SerializableVector3(pickup.transform.position);
                droppedItem.Amount = pickup.Amount;
                droppedItem.sceneBuildIndex = buildIndex;
                droppedItemsList.Add(droppedItem);
            }

            droppedItemsList.AddRange(_itemsDroppedInOtherScenes);
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            ClearDroppedItemsList();
            _itemsDroppedInOtherScenes.Clear();

            var dropRecords = (List<DropRecord>)state;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            foreach (var item in dropRecords)
            {
                if (item.sceneBuildIndex != buildIndex)
                {
                    _itemsDroppedInOtherScenes.Add(item);
                    continue;
                }

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
