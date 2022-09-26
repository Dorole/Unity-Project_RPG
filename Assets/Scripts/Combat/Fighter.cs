using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        
        [SerializeField] float _timeBetweenAttacks = 0.5f;
        [SerializeField] Transform _handTransform = null;
        [SerializeField] Weapon_SO _weapon = null;
        
        Health _target;
        Mover _mover;
        ActionScheduler _scheduler;
        Animator _animator;

        float _timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead) return;

            _mover.MoveTo(_target.transform.position, 1f);

            if (IsInRange())
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        void SpawnWeapon()
        {
            if (_weapon == null) return;
            _weapon.Spawn(_handTransform, _animator);
        }

        void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _weapon.WeaponRange;
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null)
                return false;

            Health targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject target)
        {
            _scheduler.StartAction(this);
            _target = target.GetComponent<Health>();
        }
        
        void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        //animation event
        void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(_weapon.Damage);
        }
    }
}
