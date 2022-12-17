using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("UI/Inventory Item"))]
    public class SO_InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving/loading.")]
        [SerializeField] string _itemID = null;
        [SerializeField] string _displayName = null;
        [SerializeField] [TextArea] string _description = null;
        [SerializeField] Sprite _icon = null;
        [SerializeField] Pickup _pickup = null;
        [SerializeField] bool _stackable = false;

        static Dictionary<string, SO_InventoryItem> _itemLookupCache;

        public string ItemID => _itemID;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public bool IsStackable => _stackable;

        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(_pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        #region SAVING

        /// <summary>
        /// Get the inventory item instance from its UUID.
        /// </summary>
        /// <param name="itemID">String UUID that persists between game instances.</param>
        /// <returns>Inventory item instance corresponding to the ID.</returns>
        public static SO_InventoryItem GetItemFromID(string itemID)
        {
            if (_itemLookupCache == null)
            {
                _itemLookupCache = new Dictionary<string, SO_InventoryItem>();
                var itemList = Resources.LoadAll<SO_InventoryItem>("");

                foreach (var item in itemList)
                {
                    if (_itemLookupCache.ContainsKey(item._itemID))
                    {
                        Debug.LogError(string.Format("There's a duplicate ID for objects: {0} and {1}.", _itemLookupCache[item._itemID], item));
                        continue;
                    }

                    _itemLookupCache[item._itemID] = item;
                }
            }

            if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
            return _itemLookupCache[itemID];
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(_itemID))
                _itemID = System.Guid.NewGuid().ToString();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }

        #endregion

    }
}
