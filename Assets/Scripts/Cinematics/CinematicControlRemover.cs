using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject _player;
        PlayableDirector _pd;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _pd = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            _pd.played += DisableControl;
            _pd.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
