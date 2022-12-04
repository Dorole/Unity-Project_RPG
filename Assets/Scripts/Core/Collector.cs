using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core
{
    public class Collector : MonoBehaviour, IAction
    {
        [SerializeField] float _maxPickupDistance = 3.0f;

        WeaponPickup _pickup; //should make general pickup once other items are introduced
        Mover _mover;
        ActionScheduler _actionScheduler;

        void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (_pickup == null) return;

            if (Vector3.Distance(transform.position, _pickup.transform.position) > _maxPickupDistance)
            {
                _mover.MoveTo(_pickup.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                _pickup.Pickup(gameObject);
                _pickup = null;
            }

        }

        public void Collect(WeaponPickup pickup)
        {
            _actionScheduler.StartAction(this);
            _pickup = pickup;
        }

        public void Cancel()
        {
            _pickup = null;
        }
    }
}
