using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float           regenerationPercentage = 70f;
        [SerializeField] private TakeDamageEvent takeDamage             = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> {}

        private float healthPoints = -1f;
        private bool isDead = false;

        private void Awake() {
            if (healthPoints < 0f) {
                healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth() {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(regenHealthPoints, healthPoints);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            takeDamage.Invoke(damage);
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealth() {
            return healthPoints;
        }

        public float GetMaxHealth() {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
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
