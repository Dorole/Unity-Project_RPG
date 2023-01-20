using RPG.Core.UI.Dragging;
using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
    public class LootSlotUI : MonoBehaviour, IItemHolder, IPointerClickHandler, IDragSource<SO_InventoryItem> 
    {
        //should be source? - do I want to enable dragging?

        Inventory _playerInventory;
        Loot _loot;
        int _index;
        [SerializeField] InventoryItemIcon _icon = null;

        //Setup method - will be called by LootUI (see InventorySlotUI - InventoryUI)
        //LootUI redraws when Loot object is opened

        private void Start()
        {
            _playerInventory = Inventory.GetPlayerInventory();
            //subscribe to LootUpdated - update slot(s)
        }

        public void Setup(Loot loot, int index)
        {
            _loot = loot;
            _index = index;
            _icon.SetItem(_loot.GetItemInSlot(_index), _loot.GetItemAmountInSlot(_index));
        }

        void UpdateIcon()
        {

        }

        public SO_InventoryItem GetItem()
        {
            return _loot.GetItemInSlot(_index);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //get item on this index from the Loot
            //_playerInventory.AddToFirstEmptySlot(_item, _amount);
            throw new System.NotImplementedException();
        }

        public int GetNumber()
        {
            return _loot.GetItemAmountInSlot(_index);
        }

        public void RemoveItems(int number)
        {
            //_loot.RemoveFromSlot(_index, number)
            throw new System.NotImplementedException();
        }
    }
}
