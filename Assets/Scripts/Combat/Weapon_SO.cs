using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon_SO : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController _animatorOverride = null;
        [SerializeField] GameObject _equippedPrefab = null;
        [SerializeField] float _damage = 5;
        public float Damage => _damage;
        [SerializeField] float _weaponRange = 2;
        public float WeaponRange => _weaponRange;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;
        GameObject _spawnedInstance;

        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                _spawnedInstance = Instantiate(_equippedPrefab, handTransform);
            }

            if (_animatorOverride != null)
                animator.runtimeAnimatorController = _animatorOverride;

            return _spawnedInstance;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectile = Instantiate(_projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target, _damage);
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
