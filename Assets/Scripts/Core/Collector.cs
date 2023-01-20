using RPG.Inventories;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core
{
    public class Collector : MonoBehaviour, IAction
    {
        [SerializeField] float _maxPickupDistance = 3.0f;

        ICollectable _collectable;
        Transform _collectableTransform;

        Mover _mover;
        ActionScheduler _actionScheduler;


        void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (_collectable == null) return;

            if (Vector3.Distance(transform.position, _collectableTransform.position) > _maxPickupDistance)
            {
                _mover.MoveTo(_collectableTransform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                _collectable.HandleCollection();
                Cancel();
            }

        }

        public void Collect (ICollectable collectable)
        {
            _actionScheduler.StartAction(this);
            _collectable = collectable;
            _collectableTransform = collectable.GetTransform();
        }

        public void Cancel()
        {
            _collectable = null;
            _collectableTransform = null;
        }
    }
}
