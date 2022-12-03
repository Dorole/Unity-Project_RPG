using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 5f;
        [SerializeField] float _aggroCooldownTime = 5f;
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

        LazyValue<Vector3> _guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float _timeSinceAggravated = Mathf.Infinity;
        int _currentWaypointIndex;

        void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _scheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();

            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        void Start()
        {
            _guardPosition.ForceInitialization();
        }

        void Update()
        {
            if (_health.IsDead) return;

            if (IsAggravated() && _fighter.CanAttack(_player)) 
                AttackBehaviour();

            else if (_timeSinceLastSawPlayer < _suspicionTime)
                SuspicionBehaviour();

            else
                PatrolBehaviour();

            UpdateTimers();
        }

        public void Aggravate()
        {
            _timeSinceAggravated = 0;
        }

        void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggravated += Time.deltaTime;
        }

        bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < _chaseDistance || _timeSinceAggravated < _aggroCooldownTime;
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition.value;

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
