using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] int _level = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;

        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, _level);
        }
    }
}
