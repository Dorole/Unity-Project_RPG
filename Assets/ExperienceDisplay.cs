using UnityEngine;
using TMPro;
using System;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience _experience;
        TextMeshProUGUI _xpText;

        private void Awake()
        {
            _experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            _xpText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _xpText.text = String.Format("{0:0}", _experience.ExperiencePoints);
        }
    }
}
