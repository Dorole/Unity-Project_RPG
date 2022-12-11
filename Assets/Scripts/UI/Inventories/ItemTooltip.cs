using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _titleText = null;
        [SerializeField] TextMeshProUGUI _bodyText = null;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Setup(SO_InventoryItem item)
        {
            _titleText.text = item.DisplayName;
            _bodyText.text = item.Description;
        }
    }
}
