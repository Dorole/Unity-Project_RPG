using RPG.Movement;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover;
        Fighter _fighter;
        Camera _camera;

        void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _camera = Camera.main;
        }

        void Update()
        {
            if (PerformCombat()) return;
            if (PerformMovement()) return;
        }

        bool PerformMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(hit.point);

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
