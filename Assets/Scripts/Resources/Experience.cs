using RPG.Saving;
using UnityEngine;

namespace RPG.Resources 
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0.0f;

        public void GainExperience(float experience) {
            experiencePoints += experience;
        }

        public float GetExperience() {
            return experiencePoints;
        }

        public object CaptureState() {
            return experiencePoints;
        }

        public void RestoreState(object state) {
            float xp = (float) state;
            experiencePoints = xp;
        }
    }
}