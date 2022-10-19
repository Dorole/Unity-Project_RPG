using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float _experiencePoints = 0;
        public float ExperiencePoints => _experiencePoints;

        public void GainExperience (float experience)
        {
            _experiencePoints += experience;
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
