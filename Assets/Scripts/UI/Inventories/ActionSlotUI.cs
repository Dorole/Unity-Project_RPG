using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<SO_InventoryItem>, IPointerClickHandler
    {
        [SerializeField] InventoryItemIcon _icon = null;
        [SerializeField] int _index = 0;

        ActionStore _store;

        private void Awake()
        {
            _store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            _store.OnStoreUpdated += UpdateIcon;
        }

        public void AddItems(SO_InventoryItem item, int number)
        {
            _store.AddAction(item, _index, number);
        }

        public SO_InventoryItem GetItem()
        {
            return _store.GetAction(_index);
        }

        public int GetNumber()
        {
            return _store.GetNumber(_index);
        }

        public int MaxAcceptable(SO_InventoryItem item)
        {
            return _store.MaxAcceptable(item, _index);
        }

        public void RemoveItems(int number)
        {
            _store.RemoveItems(_index, number);
        }

        void UpdateIcon()
        {
            _icon.SetItem(GetItem(), GetNumber());
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_store.GetAction(_index) == null) return;

            _store.Use(_index, null);
        }
    }
}
