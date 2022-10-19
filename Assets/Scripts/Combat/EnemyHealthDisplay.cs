using System;
using RPG.Attributes;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter _fighter;
        Health _target;
        TextMeshProUGUI _healthText;

        private void Awake()
        {
            _fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            _healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _target = _fighter.GetTarget();

            if (_target)
                _healthText.text = String.Format("{0:0}%", _target.GetPercentage());
            else
                _healthText.text = "N/A";
        }
    }
}
