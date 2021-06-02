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

        private int currentLevel = 1;

        private void Start() {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int LastLevel = CalculateLevel();
            if (currentLevel < LastLevel) {
                currentLevel = LastLevel;
                print("Levelled Up!");
            }
        }

        public float GetStat(Stats stat) {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel() {
            return currentLevel;
        }

        public int CalculateLevel() {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXp = experience.GetExperience();
            int penultimateLevel = progression.GetLevelsLength(characterClass, Stats.ExperienceToLevelUp);
            for (int level = 1; level <= penultimateLevel; level++) {
                float XPToLevelUp = progression.GetStat(characterClass, Stats.ExperienceToLevelUp, level);
                if (XPToLevelUp > currentXp) {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
