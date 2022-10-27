using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] CursorType_SO _noneCursor;
        [SerializeField] CursorType_SO _combatCursor;
        [SerializeField] CursorType_SO _movementCursor;
        [SerializeField] CursorType_SO _UICursor;

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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        _UICursor.SetCursor();
                        return true;
                    }
                }
            }
            return false;
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(hit.point, 1f);

                _movementCursor.SetCursor();
                return true;
            }

            return false;
        }

        
        
        Ray GetMouseRay()
        {
            return _camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
