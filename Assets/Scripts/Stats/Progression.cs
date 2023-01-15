using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "New Progression", menuName = "Stats/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        // consider separate progression SOs for each character class, instead of having it all in 1
        [SerializeField] ProgressionCharacterClass[] _characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;

        public float GetStat(Stat stat, CharacterClass charClass, int level)
        {
            BuildLookup();

            float[] levels = _lookupTable[charClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var progressionCharClass in _characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (var progressionStat in progressionCharClass.Stats)
                {
                    statLookupTable[progressionStat.Stat] = progressionStat.Levels;
                }

                _lookupTable[progressionCharClass.CharacterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass charClass)
        {
            BuildLookup();

            float[] levels = _lookupTable[charClass][stat];
            return levels.Length;
        }

    }

    [System.Serializable]
    internal class ProgressionCharacterClass
    {
        public CharacterClass CharacterClass;
        public ProgressionStat[] Stats;
    }

    [System.Serializable]
    internal class ProgressionStat
    {
        public Stat Stat;
        public float[] Levels;
    }
}
