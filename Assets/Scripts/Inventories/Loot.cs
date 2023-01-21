using RPG.Control;
using RPG.Core;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RPG.Inventories
{
    /// <summary>
    /// Place on the object/character that can be looted.
    /// </summary>
    public class Loot : MonoBehaviour, IRaycastable, ICollectable
    {
        //switch to internal where possible!

        public static event Action<Loot> OnLootClicked;
        public static event Action<Loot> OnLootUpdated;

        [SerializeField] DropLibrary _lootLibrary;
        public DropLibrary LootLibrary => _lootLibrary;

        [SerializeField] LootSlot[] _lootSlots;

        Inventory _inventory;
        LootConfig _lootConfig;
        bool _filled = false;

        [System.Serializable]
        public struct LootSlot
        {
            public SO_InventoryItem Item;
            public int Amount;
        }

        void Start()
        {
            _inventory = Inventory.GetPlayerInventory();
            _lootConfig = LootConfig.GetLootConfig();

            _lootSlots = new LootSlot[_lootConfig.LootSize];
        }

        void FillLoot()
        {
            if (!TryGetComponent(out BaseStats baseStats))
                baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();

            var drops = _lootLibrary.GetRandomDrops(baseStats.GetLevel());

            int i = 0;
            foreach (var drop in drops)
            {
                _lootSlots[i].Item = drop.Item;
                _lootSlots[i].Amount = drop.Amount;

                i++;

                if (i >= _lootConfig.LootSize) return;
            }

            _filled = true;
        }

        internal void RemoveLootFromSlot(int slot)
        {
            _lootSlots[slot].Item = null;
            _lootSlots[slot].Amount = 0;

            OnLootUpdated?.Invoke(this);
        }

        internal SO_InventoryItem GetItemInSlot(int slot)
        {
            return _lootSlots[slot].Item;
        }

        internal int GetItemAmountInSlot(int slot)
        {
            return _lootSlots[slot].Amount;
        }

        internal void TransferToInventory(int slot)
        {
            if (_lootSlots[slot].Item == null) return;

            if (!_inventory.HasSpaceFor(_lootSlots[slot].Item))
            {
                Debug.Log("Inventory full!");
                return;
            }

            _inventory.AddToFirstEmptySlot(_lootSlots[slot].Item, _lootSlots[slot].Amount);

            RemoveLootFromSlot(slot);
        }

        public CursorType_SO GetCursorType()
        {
            if (HasLoot() || !_filled)
                return _lootConfig.FullCursor;
            else
                return _lootConfig.EmptyCursor;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Collector>().Collect(this);

                if (!_filled)
                    FillLoot(); //call this from somewhere else??
            }

            return true;
        }

        bool HasLoot()
        {
            for (int i = 0; i < _lootSlots.Length; i++)
            {
                if (_lootSlots[i].Item != null)
                    return true;
            }

            return false;
        }

        public void HandleCollection()
        {
            Debug.Log("Open loot panel");
            OnLootClicked?.Invoke(this);
        }

        public Transform GetTransform()
        {
            return gameObject.transform;
        }

        internal void AddToLoot(SO_InventoryItem item, int number, int index)
        {
            _lootSlots[index].Item = item;

            if (item.IsStackable)
                _lootSlots[index].Amount += number;
            else
                _lootSlots[index].Amount = number;

            OnLootUpdated?.Invoke(this);
        }
    }
}
