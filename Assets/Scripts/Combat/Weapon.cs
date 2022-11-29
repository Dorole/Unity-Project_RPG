using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent _onHit;

        public void OnHit()
        {
            _onHit.Invoke();
        }
    }
}
