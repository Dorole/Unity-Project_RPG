using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float _healthPoints = 20f;

        Animator _anim;
        bool _isDead;
        public bool IsDead => _isDead;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            print(_healthPoints);

            if (_healthPoints <= 0)
                Die();
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _anim.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
