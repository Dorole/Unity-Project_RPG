using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Cursors")]
        [SerializeField] CursorType_SO _noneCursor;
        [SerializeField] CursorType_SO _combatCursor;
        [SerializeField] CursorType_SO _movementCursor;
        [SerializeField] CursorType_SO _UICursor;
        [Header("NavMesh Config")]
        [SerializeField] float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] float _raycastRadius = 0.2f;

        Mover _mover;
        Fighter _fighter;
        Health _health;
        Camera _camera;

        void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _camera = Camera.main;
        }

        void Update()
        {
            if (InteractWithUI()) return;

            if (_health.IsDead)
            {
                _noneCursor.SetCursor();
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            _noneCursor.SetCursor();
        }

        bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                _UICursor.SetCursor();
                return true;
            }
            return false;
        }

        bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        raycastable.GetCursorType().SetCursor();
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), _raycastRadius);
            float[] distances = new float[hits.Length];

            //sort hits by their distance
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(target, 1f);

                _movementCursor.SetCursor();
                return true;
            }

            return false;
        }

        
        
        bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        Ray GetMouseRay()
        {
            return _camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
