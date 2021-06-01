using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float healthPoints = -1f;
        private bool isDead = false;

        private void Start() {
            if (healthPoints < 0) {
                healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage() {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
        }

        private void AwardExperience(GameObject instigator) {
            Experience expComponent = instigator.GetComponent<Experience>();
            if (expComponent) {
                expComponent.GainExperience(GetComponent<BaseStats>().GetStat(Stats.Stats.ExperienceReward));
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float healthState = (float) state;
            healthPoints = healthState;
            if (healthPoints <= 0) {
                Die();
                GetComponent<Animator>().Play("Death");
            }
        }
    }
}
