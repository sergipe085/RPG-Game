using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] private int            startingLevel  = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Player;
        [SerializeField] private Progression    progression    = null;

        private void Update() {
            if (gameObject.tag == "Player") {
                print(GetLevel());
            }
        }

        public float GetStat(Stats stat) {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        private int GetLevel() {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXp = experience.GetExperience();
            
            int currentLevel = 1;
            for (int i = 1; i <= progression.GetLevelsLength(characterClass, Stats.ExperienceToLevelUp); i++) {
                if (currentXp >= progression.GetStat(characterClass, Stats.ExperienceToLevelUp, i)) {
                    currentLevel = i + 1;
                }
            }
            return currentLevel;
        }
    }
}
