using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class InventoryItemIcon : MonoBehaviour
    {
        internal void SetItem(Sprite item)
        {
            var iconImage = GetComponent<Image>();

            if (item == null)
                iconImage.enabled = false;
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item;
            }    
        }

        internal Sprite GetItem()
        {
            var iconImage = GetComponent<Image>();

            if (!iconImage.enabled)
                return null;

            return iconImage.sprite;
        }
    }
}
