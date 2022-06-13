using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG
{
    public class Mover : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        Camera _camera;
        Animator _animator;

        void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _camera = Camera.main;
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
                MoveToMousePosition();

            UpdateAnimator();
        }

        void MoveToMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
                _navMeshAgent.SetDestination(hit.point);
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
