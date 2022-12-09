using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI _inventorySlotPrefab = null;

        Inventory _playerInventory;

        void Awake()
        {
            _playerInventory = Inventory.GetPlayerInventory();
            _playerInventory.OnInventoryUpdated += Redraw;
        }

        void Start()
        {
            Redraw();
        }

        void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.GetInventorySize(); i++)
            {
                var slotUI = Instantiate(_inventorySlotPrefab, transform);
                slotUI.Setup(_playerInventory, i);
            }
        }
    }
}
