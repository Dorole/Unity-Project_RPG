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

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (_equippedPrefab != null)
                Instantiate(_equippedPrefab, handTransform);

            if (_animatorOverride != null)
                animator.runtimeAnimatorController = _animatorOverride;
        }
    }
}
