using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Inventory/Stats Equippable Item"))]
    public class SO_StatsEquippableItem : SO_EquippableItem, IModifierProvider
    {
        [System.Serializable]
        struct Modifier
        {
            public Stat Stat;
            public float Value;
        }

        [SerializeField] Modifier[] _additiveModifiers;
        [SerializeField] Modifier[] _percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in _additiveModifiers)
            {
                if (modifier.Stat == stat)
                    yield return modifier.Value;
            }   
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in _percentageModifiers)
            {
                if (modifier.Stat == stat)
                    yield return modifier.Value;
            }
        }
    }
}
