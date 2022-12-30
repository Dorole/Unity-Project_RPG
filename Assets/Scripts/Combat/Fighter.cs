using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using RPG.Utils;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float _timeBetweenAttacks = 0.5f;
        [SerializeField] Transform _rightHand = null;
        [SerializeField] Transform _leftHand = null;
        [SerializeField] WeaponConfig_SO _defaultWeapon = null;

        WeaponConfig_SO _currentWeaponConfig;
        LazyValue<Weapon> _currentWeapon;
        
        Health _target;
        Mover _mover;
        ActionScheduler _scheduler;
        Animator _animator;
        BaseStats _baseStats;
        Equipment _equipment;

        float _timeSinceLastAttack = Mathf.Infinity;

        void Awake()
        {
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _equipment = GetComponent<Equipment>();

            if (_equipment) 
                _equipment.OnEquipmentUpdated += UpdateWeapon;

            _currentWeaponConfig = _defaultWeapon;
            _currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);
        }

        Weapon SetUpDefaultWeapon()
        {
            return AttachWeapon(_defaultWeapon);
        }

        void Start()
        {
            _currentWeapon.ForceInitialization();
        }

        void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead) 
                return;

            _mover.MoveTo(_target.transform.position, 1f);

            if (IsInRange(_target.transform))
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig_SO weapon)
        {
            if (_currentWeapon.value)
                UnequipWeapon();

            _currentWeaponConfig = weapon;
            _currentWeapon.value = AttachWeapon(weapon);
        }

        void UpdateWeapon()
        {
            var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig_SO;
            if (weapon == null)
                EquipWeapon(_defaultWeapon);
            else
                EquipWeapon(weapon);
        }

        Weapon AttachWeapon(WeaponConfig_SO weapon)
        {
           return _currentWeapon.value = weapon.Spawn(_rightHand, _leftHand, _animator);
        }

        void UnequipWeapon()
        {
            Destroy(_currentWeapon.value.gameObject);
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

        bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _currentWeaponConfig.WeaponRange;
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;

            if (!_mover.CanMoveTo(target.transform.position) &&
                !IsInRange(target.transform))
            {
                return false;
            }

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

        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string savedWeapon = (string)state;
            WeaponConfig_SO weapon = Resources.Load<WeaponConfig_SO>(savedWeapon);
            EquipWeapon(weapon);
        }

        //animation events
        void Hit()
        {
            if (_target == null) return;

            float damage = _baseStats.GetStat(Stat.Damage);

            if (_currentWeapon.value != null)
                _currentWeapon.value.OnHit();

            if (_currentWeaponConfig.HasProjectile())
                _currentWeaponConfig.LaunchProjectile(_rightHand, _leftHand, _target, damage);
            else
            {
                _target.TakeDamage(damage);
            }
        }

        void Shoot() 
        {
            Hit();
        }
    }
}
