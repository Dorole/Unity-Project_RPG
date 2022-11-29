using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public UnityEvent<float> OnTakeDamage;
        public UnityEvent OnDie;

        [SerializeField] float _regenerationPercentage = 100;

        LazyValue<float> _healthPoints;
        public float HealthPoints => _healthPoints.value;

        BaseStats _baseStats;
        Animator _anim;
        bool _isDead;
        public bool IsDead => _isDead;


        void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _anim = GetComponent<Animator>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        void OnEnable()
        {
            if (gameObject.CompareTag("Player"))
                _baseStats.OnLevelUp += RegenerateHealth;
        }

        private void Start()
        {
            //if at this point nothing called on healthPoints and initialized it,
            //do it now
            _healthPoints.ForceInitialization();
        }

        public void TakeDamage(float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);

            OnTakeDamage.Invoke(damage);

            if (_healthPoints.value <= 0)
            {
                OnDie.Invoke();
                AwardExperience();
                Die();
            }
        }

        void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _anim.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;

            if (_healthPoints.value <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Use this to display health in the percentage format.
        /// </summary>
        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return _healthPoints.value / _baseStats.GetStat(Stat.Health);
        }

        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public void Heal(float healthPointsToRestore)
        {
            float healedHealth = _healthPoints.value + healthPointsToRestore;
            _healthPoints.value = Mathf.Min(healedHealth, GetMaxHealthPoints());
        }

        void AwardExperience()
        {
            if (gameObject.CompareTag("Player")) return;

            Experience playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            playerExperience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        void RegenerateHealth()
        {
            float regenHealthPoints = _baseStats.GetStat(Stat.Health) * (_regenerationPercentage / 100);
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPoints);
            //_healthPoints.value = _baseStats.GetStat(Stat.Health);
        }

        void OnDisable()
        {
            if (gameObject.CompareTag("Player"))
                _baseStats.OnLevelUp -= RegenerateHealth;
        }
    }
}
