using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 5f;

        GameObject _player;
        Fighter _fighter;
        Mover _mover;
        ActionScheduler _scheduler;
        Health _health;

        Vector3 _guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
            _guardPosition = transform.position;
        }

        void Update()
        {
            if (_health.IsDead) return;

            //at the end come back and refactor using FSM
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
                SuspicionBehaviour();
            else
                GuardBehaviour();

            _timeSinceLastSawPlayer += Time.deltaTime;
        }
        
        bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < _chaseDistance;
        }

        void GuardBehaviour()
        {
            _mover.StartMoveAction(_guardPosition);
        }

        void SuspicionBehaviour()
        {
            _scheduler.CancelCurrentAction();
        }

        void AttackBehaviour()
        {
            _fighter.Attack(_player);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance); 
        }
    }
}
