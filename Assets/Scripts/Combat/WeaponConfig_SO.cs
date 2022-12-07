using RPG.Attributes; 
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig_SO : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController _animatorOverride = null;
        [SerializeField] Weapon _equippedPrefab = null;
        [SerializeField] float _damage = 5;
        public float Damage => _damage;
        [SerializeField] float _percentageBonus = 0;
        public float PercentageBonus => _percentageBonus;
        [SerializeField] float _weaponRange = 2;
        public float WeaponRange => _weaponRange;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;
        Weapon _spawnedInstance;

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                _spawnedInstance = Instantiate(_equippedPrefab, handTransform);
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride != null)
                animator.runtimeAnimatorController = _animatorOverride;
            else if (overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

            return _spawnedInstance;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, float damage)
        {
            Projectile projectile = Instantiate(_projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target, damage);
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }
        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return _isRightHanded ? rightHand : leftHand;
        }
    }
}