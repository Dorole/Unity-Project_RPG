using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IDragContainer<SO_InventoryItem>, IItemHolder
    {
        [SerializeField] InventoryItemIcon _icon = null;
        [SerializeField] EquipLocation _equipLocation = EquipLocation.Weapon;

        Equipment _equipment;

        void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _equipment = player.GetComponent<Equipment>();
            _equipment.OnEquipmentUpdated += RedrawUI;
        }

        private void Start()
        {
            RedrawUI();
        }

        public void AddItems(SO_InventoryItem item, int number)
        {
            _equipment.AddItem(_equipLocation, (SO_EquippableItem)item);
        }

        public SO_InventoryItem GetItem()
        {
            return _equipment.GetItemInSlot(_equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
                return 1;
            else
                return 0;
        }

        public int MaxAcceptable(SO_InventoryItem item)
        {
            SO_EquippableItem equippableItem = item as SO_EquippableItem;

            if (equippableItem == null) return 0;

            if (equippableItem.AllowedEquipLocation != _equipLocation)
                return 0;

            if (GetItem() != null)
                return 0;

            return 1;
        }

        public void RemoveItems(int number)
        {
            _equipment.RemoveItem(_equipLocation);
        }

        void RedrawUI()
        {
            _icon.SetItem(_equipment.GetItemInSlot(_equipLocation));
        }
    }
}
