using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] bool _isHoming = false;
        [SerializeField] float _speed = 1f;
        [SerializeField] GameObject _hitEffect = null;
        Health _target = null;
        float _damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if (_isHoming && !_target.IsDead)
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * _speed * Time.deltaTime); //vidi lekciju 117 Q&A za razdvajanje homing i straight shoot logike
        }


        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = _target.GetComponent<CapsuleCollider>();

            if (targetCollider == null) 
                return _target.transform.position;

            return _target.transform.position + (Vector3.up * targetCollider.height / 2);
        }
        
        void OnTriggerEnter(Collider other)
        {
            //if (other.GetComponent<Health>() != _target) return;
            //if (_target.IsDead) return;

            Health target = other.GetComponent<Health>();

            if (target == null)
                gameObject.SetActive(false);

            if (target != null && !target.IsDead)
            {
                target.TakeDamage(_damage);

                if (_hitEffect != null)
                    Instantiate(_hitEffect, GetAimLocation(), transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }
}
