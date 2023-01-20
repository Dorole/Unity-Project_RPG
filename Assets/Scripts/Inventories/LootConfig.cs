using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// Configuration of fields that are common to all loot. 
    /// </summary>
    public class LootConfig : MonoBehaviour
    {
        [SerializeField] int _lootSize = 5;
        [SerializeField] CursorType_SO _fullCursor = null;
        [SerializeField] CursorType_SO _emptyCursor = null;
        
        public int LootSize => _lootSize;
        public CursorType_SO FullCursor => _fullCursor;
        public CursorType_SO EmptyCursor => _emptyCursor;

        public static LootConfig GetLootConfig()
        {
            var configCheck = FindObjectsOfType<LootConfig>();
            if (configCheck.Length == 1)
            {
                return configCheck[0];
            }
            else
            {
                Debug.LogError("There is either 0 or more than 1 LootConfigs in the scene. There should be only one.");
                return null;
            }
        }
    }
}
