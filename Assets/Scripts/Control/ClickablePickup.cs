using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] CursorType_SO _pickupCursor = null;
        [SerializeField] CursorType_SO _inventoryFullCursor = null;
        Pickup _pickup;

        void Awake()
        {
            _pickup = GetComponent<Pickup>();
        }

        public CursorType_SO GetCursorType()
        {
            if (_pickup.CanBePickedUp())
                return _pickupCursor;
            else
                return _inventoryFullCursor;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Collector>().Collect(_pickup);
            }
            return true;
        }

    }
}
