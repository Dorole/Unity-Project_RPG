using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Inventory/Action Item"))]
    public class SO_ActionItem : SO_InventoryItem
    {
        [SerializeField] bool _consumable = false;
        public bool IsConsumable => _consumable;

        [Tooltip("Describe effect prior to implementing")]
        [SerializeField] [TextArea] string _placeholderMessage = null;

        public virtual void Use(GameObject target)
        {
            Debug.Log($"{this} : {_placeholderMessage}");
        }
    }
}
