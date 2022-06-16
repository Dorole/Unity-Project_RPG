using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float _weaponRange = 2f;
        [SerializeField] float _timeBetweenAttacks = 0.5f;
        [SerializeField] float _damage = 5;
        
        Transform _target;
        Mover _mover;
        ActionScheduler _scheduler;
        Animator _animator;
        Health _targetHealth;

        float _timeSinceLastAttack;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;

            _mover.MoveTo(_target.position);

            if (IsInRange())
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        void AttackBehaviour()
        {
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                _animator.SetTrigger("attack");
                _timeSinceLastAttack = 0;
            }
        }

        bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        public void Attack(CombatTarget target)
        {
            _scheduler.StartAction(this);
            _target = target.transform;
            _targetHealth = _target.GetComponent<Health>();
        }

        public void Cancel()
        {
            _target = null;
        }

        //animation event
        void Hit()
        {
            _targetHealth.TakeDamage(_damage);
        }
    }
}
