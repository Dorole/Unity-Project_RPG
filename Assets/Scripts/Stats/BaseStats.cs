using RPG.Utils;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action OnLevelUp;

        [Range(0, 99)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;
        [SerializeField] GameObject _levelUpParticleEffect = null;
        [SerializeField] bool _shouldUseMods = false;

        LazyValue<int> _currentLevel;
        Experience _experience;

        void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        void OnEnable()
        {
            if (_experience != null)
                _experience.OnExperienceGained += UpdateLevel;
        }

        void OnDisable()
        {
            if (_experience != null)
                _experience.OnExperienceGained -= UpdateLevel;
        }

        void Start()
        {
            _currentLevel.ForceInitialization();
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                PlayLevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private void PlayLevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifiers(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            //it will definitely be set if it isn't already
            //if (_currentLevel.value < 1)
            //    _currentLevel.value = CalculateLevel();

            return _currentLevel.value;
        }

        int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return _startingLevel;

            float currentXP = experience.ExperiencePoints;
            int penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, _characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPtoLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                
                if (XPtoLevelUp > currentXP)
                    return level;
            }

            return penultimateLevel + 1;
        }

        float GetAdditiveModifiers(Stat stat)
        {
            if (!_shouldUseMods) return 0;

            var modProviders = GetComponents<IModifierProvider>();
            float total = 0;

            foreach (var modProvider in modProviders)
            {
                foreach (float mod in modProvider.GetAdditiveModifiers(stat))
                {
                    total += mod;
                }
            }

            return total;
        }

        float GetPercentageModifiers(Stat stat)
        {
            if (!_shouldUseMods) return 0;

            var modProviders = GetComponents<IModifierProvider>();
            float total = 0;

            foreach (var modProvider in modProviders)
            {
                foreach (float mod in modProvider.GetPercentageModifiers(stat))
                {
                    total += mod;
                }
            }

            return total;
        }
    }
}
