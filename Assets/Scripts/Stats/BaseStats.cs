using System;
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
        [SerializeField] private bool shouldUseModifiers       = false;
        public event Action onLevelUp;

        [Header("EFFECTS")]
        [SerializeField] private GameObject levelUpEffect = null;

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
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stats stat) {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stats stat) {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel() {
            return currentLevel;
        }

        private int CalculateLevel() {
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

        private float GetAdditiveModifier(Stats stat) {
            if (!shouldUseModifiers) return 0.0f;

            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            float additive = 0.0f;

            foreach(IModifierProvider modifierProvider in modifierProviders) {
                foreach(float f in modifierProvider.GetAdditiveModifier(stat)) {
                    additive += f;
                }
            }

            return additive;
        }

        private float GetPercentageModifier(Stats stat) {
            if (!shouldUseModifiers) return 0.0f;

            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            float additive = 0.0f;

            foreach (IModifierProvider modifierProvider in modifierProviders) {
                foreach (float f in modifierProvider.GetPercentageModifier(stat)) {
                    additive += f;
                }
            }

            return additive;
        }
    }
}
