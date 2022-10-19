using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 5f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float _waypointTolerance = 1f;
        [SerializeField] float _waypointDwellTime = 2f;
        [Range(0,1)]
        [SerializeField] float _patrolSpeedFraction = 0.2f; //fraction of maxSpeed from Mover (20%)

        GameObject _player;
        Fighter _fighter;
        Mover _mover;
        ActionScheduler _scheduler;
        Health _health;

        Vector3 _guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int _currentWaypointIndex;

        void Awake()
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
                AttackBehaviour();

            else if (_timeSinceLastSawPlayer < _suspicionTime)
                SuspicionBehaviour();

            else
                PatrolBehaviour();

            UpdateTimers();
        }

        void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < _chaseDistance;
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;

            if (_patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
        }

        Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        void SuspicionBehaviour()
        {
            _scheduler.CancelCurrentAction();
        }

        void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance); 
        }
    }
}
