using UnityEngine;

namespace RPG.Audio
{
    //simple placeholder implementation
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] _audioClips;
        [Range(-3, 3)] [SerializeField] float _pitchMin = 0.7f;
        [Range(-3, 3)] [SerializeField] float _pitchMax = 1.2f;
        
        AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _audioClips[0];

            if (_audioSource.playOnAwake)
                PlayClip();
        }

        public void PlayClip()
        {
            if (_audioClips.Length > 1)
                PlayRandomClip();

            _audioSource.pitch = Random.Range(Mathf.Min(_pitchMin, _pitchMax), Mathf.Max(_pitchMin, _pitchMax));
            _audioSource.Play();
        }

        void PlayRandomClip()
        {
            AudioClip previousClip = _audioSource.clip;

            do
            {
                _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length)];
            }
            while (_audioSource.clip == previousClip);
        }
    }
}
