using RPG.Movement;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        GameObject _player;
        Fighter _fighter;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            if (InAttackRangeOfPlayer())
                _fighter.Attack(_player);
            else
                _fighter.Cancel();
        }

        bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < _chaseDistance;
        }
    }
}
