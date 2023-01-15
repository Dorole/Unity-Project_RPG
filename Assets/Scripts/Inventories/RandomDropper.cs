using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far the dropped items can be scattered from the dropper")]
        [SerializeField] float _scatterDistance = 1;
        [SerializeField] DropLibrary _dropLibrary;

        const int _ATTEMPTS = 30;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();
            var drops = _dropLibrary.GetRandomDrops(baseStats.GetLevel());
            
            foreach (var drop in drops)
                Dropitem(drop.Item, drop.Amount);
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < _ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * _scatterDistance;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                    return hit.position;
            }

            return new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        }
    }
}
