using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;

        int _currentLevel = 0;

        void Start()
        {
            _currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();

            if (experience != null)
                experience.OnExperienceGained += UpdateLevel;
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                print("Levelled up!");
            }
        }

        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (_currentLevel < 1)
                _currentLevel = CalculateLevel();

            return _currentLevel;
        }

        public int CalculateLevel()
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
    }
}
