using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool _isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _isTriggered)
                return;

            _isTriggered = true; 
            GetComponent<PlayableDirector>().Play();
        }
    }
}
