using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        
        [SerializeField] float _timeBetweenAttacks = 0.5f;
        [SerializeField] Transform _rightHand = null;
        [SerializeField] Transform _leftHand = null;
        [SerializeField] Weapon_SO _defaultWeapon = null;
        [SerializeField] string _defaultWeaponName = "Unarmed";

        LazyValue<Weapon_SO> _currentWeapon;
        GameObject _equippedWeapon;
        
        Health _target;
        Mover _mover;
        ActionScheduler _scheduler;
        Animator _animator;
        BaseStats _baseStats;

        float _timeSinceLastAttack = Mathf.Infinity;

        void Awake()
        {
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();

            _currentWeapon = new LazyValue<Weapon_SO>(SetUpDefaultWeapon);
        }

        Weapon_SO SetUpDefaultWeapon()
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
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

            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        void AttachWeapon(Weapon_SO weapon)
        {
           weapon.Spawn(_rightHand, _leftHand, _animator);
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
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.WeaponRange;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return _currentWeapon.value.Damage;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return _currentWeapon.value.PercentageBonus;
        }

        public object CaptureState()
        {
            return _currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string savedWeapon = (string)state;
            Weapon_SO weapon = Resources.Load<Weapon_SO>(savedWeapon);
            EquipWeapon(weapon);
        }

        //animation events
        void Hit()
        {
            if (_target == null) return;

            float damage = _baseStats.GetStat(Stat.Damage);
            if (_currentWeapon.value.HasProjectile())
                _currentWeapon.value.LaunchProjectile(_rightHand, _leftHand, _target, damage);
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
