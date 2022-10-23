using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action OnExperienceGained;

        [SerializeField] float _experiencePoints = 0;
        public float ExperiencePoints => _experiencePoints;

        public void GainExperience (float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained?.Invoke();
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}
