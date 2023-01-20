using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(fileName = "Drop Library", menuName = "Inventory/Drop Library", order = 0)]
    public class DropLibrary : ScriptableObject //rename Loot Library
    {
        [SerializeField] DropConfig[] _potentialDrops;
        [SerializeField] float[] _dropChance; //percentage
        [SerializeField] int[] _minDrops;
        [SerializeField] int[] _maxDrops;

        [System.Serializable]
        class DropConfig
        {
            public SO_InventoryItem Item;
            public float[] RelativeChance;
            public int[] MinItems;
            public int[] MaxItems;

            public int GetRandomNumberOfItems(int level)
            {
                if (!Item.IsStackable)
                    return 1;

                int min = GetByLevel(MinItems, level);
                int max = GetByLevel(MaxItems, level);
                return Random.Range(min, max + 1);
            }
        }

        public struct Dropped
        {
            public SO_InventoryItem Item;
            public int Amount;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            //if (!ShouldRandomDrop(level))
            //    yield break;

            List<DropConfig> potentialDropInstance = _potentialDrops.ToList();

            int numberOfDrops = GetRandomNumberOfDrops(level);
            for (int i = 0; i < numberOfDrops; i++)
                yield return GetRandomDrop(level, potentialDropInstance);

            Debug.Log($"Dropped {numberOfDrops} items");
        }

        bool ShouldRandomDrop(int level)
        {
            return Random.Range(0, 100) < GetByLevel(_dropChance, level);
        }

        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(_minDrops, level);
            int max = GetByLevel(_maxDrops, level);

            return Random.Range(min, max + 1);
        }

        Dropped GetRandomDrop(int level, List<DropConfig> potentialDropInstance)
        {
            var drop = SelectRandomItem(level, potentialDropInstance);
            if (!drop.Item.IsStackable)
                potentialDropInstance.Remove(drop);

            var result = new Dropped();
            result.Item = drop.Item;
            result.Amount = drop.GetRandomNumberOfItems(level);
            return result;
        }

        DropConfig SelectRandomItem(int level, List<DropConfig> potentialDropInstance)
        {
            float totalChance = GetTotalChance(level, potentialDropInstance);
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0;

            foreach (var drop in potentialDropInstance)
            {
                chanceTotal += GetByLevel(drop.RelativeChance, level);

                if (chanceTotal >= randomRoll)
                    return drop;
            }

            return null;
        }

        float GetTotalChance(int level, List<DropConfig> potentialDropInstance)
        {
            float total = 0;
            foreach (var drop in potentialDropInstance)
            {
                total += GetByLevel(drop.RelativeChance, level);
            }
            return total;
        }

        //static bc it doesn't need to belong to a particular instance of a class
        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
                return default;

            if (level > values.Length)
                return values[values.Length - 1];

            if (level <= 0)
                return default;

            return values[level - 1];
        }

    }
}
