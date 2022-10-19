using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        // consider separate progression SOs for each character class, instead of having it all in 1
        [SerializeField] ProgressionCharacterClass[] _characterClasses = null;

        public float GetStat(Stat stat, CharacterClass charClass, int level)
        {
            foreach (ProgressionCharacterClass pcc in _characterClasses)
            {
                if (pcc.CharacterClass != charClass) continue;

                foreach (ProgressionStat progressionStat in pcc.Stats)
                {
                    if (progressionStat.Stat != stat) continue;
                    if (progressionStat.Levels.Length < level) continue;

                    return progressionStat.Levels[level - 1];
                }
            }

            return 0;
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
