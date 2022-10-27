using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] CursorType_SO _noneCursor;
        [SerializeField] CursorType_SO _combatCursor;
        [SerializeField] CursorType_SO _movementCursor;

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
            if (_health.IsDead) return;

            if (PerformCombat()) return;
            if (PerformMovement()) return;

            _noneCursor.SetCursor();
        }

        bool PerformMovement()
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

        
        
        bool PerformCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                    _fighter.Attack(target.gameObject);

                _combatCursor.SetCursor();
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
