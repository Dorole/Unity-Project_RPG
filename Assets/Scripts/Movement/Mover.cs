using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        Animator _animator;
        Fighter _fighter;

        void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _fighter.CancelAttack();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
        }

        public void Stop()
        {
            _navMeshAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            _animator.SetFloat("forwardSpeed", speed);
        }
    }
}
