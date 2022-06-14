using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float _weaponRange = 2f;
        Transform _target;
        Mover _mover;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (_target == null) return;

            _mover.MoveTo(_target.position);

            if (IsInRange())
                _mover.Stop();
        }
        
        bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        public void Attack(CombatTarget target)
        {
            _target = target.transform;
        }

        public void CancelAttack()
        {
            _target = null;
        }

    }
}
