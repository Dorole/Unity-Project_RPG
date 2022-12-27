using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] GameObject _textContainer = null;
        [SerializeField] TextMeshProUGUI _itemAmount = null;

        public void SetItem(SO_InventoryItem item)
        {
            SetItem(item, 0);
        }

        public void SetItem(SO_InventoryItem item, int number)
        {
            var iconImage = GetComponent<Image>();

            if (item == null)
                iconImage.enabled = false;
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }    

            if (_itemAmount)
            {
                if (number <= 1)
                    _textContainer.SetActive(false);
                else
                {
                    _textContainer.SetActive(true);
                    _itemAmount.text = number.ToString();
                }
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
