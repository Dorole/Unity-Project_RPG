using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _damageText = null;

        public void SetText(float value)
        {
            _damageText.text = String.Format("{0:0}", value);
        }
    }
}
