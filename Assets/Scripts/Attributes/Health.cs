using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float _healthPoints = 20f;
        float _startingHealthPoints;

        Animator _anim;
        GameObject _player;
        bool _isDead;
        public bool IsDead => _isDead;


        private void Awake()
        {
            _startingHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            _healthPoints = _startingHealthPoints;
            
            _anim = GetComponent<Animator>();

            if (!gameObject.CompareTag("Player"))
                _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
        }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            //print(_healthPoints);

            if (_healthPoints <= 0)
            {
                AwardExperience();
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _anim.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;

            if (_healthPoints <= 0)
            {
                Die();
            }
        }

        public float GetPercentage()
        {
            return 100 * (_healthPoints / _startingHealthPoints);
        }

        void AwardExperience()
        {
            if (gameObject.CompareTag("Player")) return;

            Experience experience = _player.GetComponent<Experience>();
            //if (experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
    }
}
