using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float _timeBetweenAttacks = 0.5f;
        [SerializeField] Transform _rightHand = null;
        [SerializeField] Transform _leftHand = null;
        [SerializeField] Weapon_SO _defaultWeapon = null;
        [SerializeField] string _defaultWeaponName = "Unarmed";

        Weapon_SO _currentWeapon = null;
        GameObject _equippedWeapon;
        
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
            if (_currentWeapon == null)
                EquipWeapon(_defaultWeapon);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead) 
                return;

            _mover.MoveTo(_target.transform.position, 1f);

            if (IsInRange())
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon_SO weapon)
        {
            if (_equippedWeapon)
                UnequipWeapon();

            _currentWeapon = weapon;
            _equippedWeapon = _currentWeapon.Spawn(_rightHand, _leftHand, _animator);
        }

        void UnequipWeapon()
        {
            Destroy(_equippedWeapon.gameObject);
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
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.WeaponRange;
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

        public Health GetTarget()
        {
            return _target;
        }

        //animation events
        //names defined by asset packs so it is what it is
        void Hit()
        {
            if (_target == null) return;

            if (_currentWeapon.HasProjectile())
                _currentWeapon.LaunchProjectile(_rightHand, _leftHand, _target);
            else
                _target.TakeDamage(_currentWeapon.Damage);
        }

        void Shoot() 
        {
            Hit();
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string savedWeapon = (string)state;
            Weapon_SO weapon = Resources.Load<Weapon_SO>(savedWeapon);
            EquipWeapon(weapon);
        }
    }
}
