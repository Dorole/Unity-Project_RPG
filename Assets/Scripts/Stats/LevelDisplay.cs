using UnityEngine;
using TMPro;
using System;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats _playerStats;
        TextMeshProUGUI _levelText;

        private void Awake()
        {
            _playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _levelText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _levelText.text = String.Format("{0:0}", _playerStats.GetLevel());
        }
    }
}
