using System;
using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health _health;
        TextMeshProUGUI _healthText;

        private void Awake()
        {
            _health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            _healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            //:0 - 0 decimal places, :0.0 - 1 dec place
            _healthText.text = String.Format("{0:0}%", _health.GetPercentage()); 
        }
    }
}
