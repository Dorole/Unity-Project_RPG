using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] SO_InventoryItem _item = null;
        [SerializeField] int _amount = 1;

        void Awake()
        {
            SpawnPickup();
        }

        void SpawnPickup()
        {
            var spawnedPickup = _item.SpawnPickup(transform.position, _amount);
            spawnedPickup.transform.SetParent(transform);
        }

        Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }

        bool IsCollected()
        {
            return GetPickup() == null;
        }

        void DestroyPickup()
        {
            if (GetPickup())
                Destroy(GetPickup().gameObject);
        }

        object ISaveable.CaptureState()
        {
            return IsCollected();
        }

        void ISaveable.RestoreState(object state)
        {
            bool wasCollected = (bool)state;

            if (wasCollected && !IsCollected())
                DestroyPickup();

            if (!wasCollected && IsCollected())
                SpawnPickup();
        }
    }
}
