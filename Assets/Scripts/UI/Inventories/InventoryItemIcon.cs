using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        public void SetItem(SO_InventoryItem item)
        {
            var iconImage = GetComponent<Image>();

            if (item == null)
                iconImage.enabled = false;
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }    
        }

        public Sprite GetItem()
        {
            var iconImage = GetComponent<Image>();

            if (!iconImage.enabled)
                return null;

            return iconImage.sprite;
        }
    }
}
