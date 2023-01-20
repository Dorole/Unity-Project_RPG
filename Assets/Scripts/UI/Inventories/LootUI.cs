using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class LootUI : MonoBehaviour
    {
        [SerializeField] LootSlotUI _lootSlotPrefab = null;
        [SerializeField] GameObject _lootPanel = null;
        [SerializeField] Transform _inventoryItemsParent = null;

        LootConfig _lootConfig = null;
        Loot _loot = null;
        List<LootSlotUI> _lootSlots = new List<LootSlotUI>();

        private void OnEnable()
        {
            Loot.OnLootClicked += OpenLootPanel;
        }

        private void Start()
        {
            _lootConfig = LootConfig.GetLootConfig();
            for (int i = 0; i < (_lootConfig.LootSize); i++)
            {
                var lootSlot = Instantiate(_lootSlotPrefab, _inventoryItemsParent);
                _lootSlots.Add(lootSlot);
            }

            _lootPanel.SetActive(false);
        }

        void OpenLootPanel(Loot loot)
        {
            if (_loot != loot)
            {
                _loot = loot;

                for (int i = 0; i < _lootConfig.LootSize; i++)
                    _lootSlots[i].Setup(_loot, i);     
            }

            _lootPanel.SetActive(true);
        }

        private void OnDisable()
        {
            Loot.OnLootClicked -= OpenLootPanel;
        }
    }
}
