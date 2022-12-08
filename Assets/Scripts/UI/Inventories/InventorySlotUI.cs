using RPG.Core.UI.Dragging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
    {
        [SerializeField] InventoryItemIcon _icon = null;

        public void AddItems(Sprite item, int number)
        {
            _icon.SetItem(item);
        }

        public Sprite GetItem()
        {
            return _icon.GetItem();
        }

        public int GetNumber()
        {
            return 1;
        }

        public int MaxAcceptable(Sprite item)
        {
            if (GetItem() == null)
                return int.MaxValue;

            return 0;
        }

        public void RemoveItems(int number)
        {
            _icon.SetItem(null);
        }

    }
}
