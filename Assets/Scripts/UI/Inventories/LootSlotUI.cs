using RPG.Core.UI.Dragging;
using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
    public class LootSlotUI : MonoBehaviour, IItemHolder, IPointerClickHandler, IDragContainer<SO_InventoryItem> 
    {
        Loot _loot;
        int _index;
        [SerializeField] InventoryItemIcon _icon = null;

        private void OnEnable()
        {
            Loot.OnLootUpdated += RedrawUI;
        }

        private void OnDisable()
        {
            Loot.OnLootUpdated -= RedrawUI;
        }

        public void Setup(Loot loot, int index)
        {
            _loot = loot;
            _index = index;
            _icon.SetItem(_loot.GetItemInSlot(_index), _loot.GetItemAmountInSlot(_index));
        }

        public SO_InventoryItem GetItem()
        {
            return _loot.GetItemInSlot(_index);
        }

        void RedrawUI(Loot loot)
        {
            if (_loot != loot)
                _loot = loot;
            
            _icon.SetItem(_loot.GetItemInSlot(_index), _loot.GetItemAmountInSlot(_index));
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _loot.TransferToInventory(_index);
        }

        public int GetNumber()
        {
            return _loot.GetItemAmountInSlot(_index);
        }

        public void RemoveItems(int number)
        {
            _loot.RemoveLootFromSlot(_index);
        }

        public int MaxAcceptable(SO_InventoryItem item)
        {
            if (_loot.GetItemInSlot(_index) == null) 
                return int.MaxValue;

            if (ReferenceEquals(item, _loot.GetItemInSlot(_index)))
            {
                if (item.IsStackable)
                    return int.MaxValue;
                else
                    return 0;
            }

            else
            {
                if (item.IsStackable)
                    return int.MaxValue;
                else
                    return 1;
            }
        }

        public void AddItems(SO_InventoryItem item, int number)
        {
            _loot.AddToLoot(item, number, _index);
        }
    }
}
