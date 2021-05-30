using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;

        public float GetStat(CharacterClass _characterClass, Stats _stats, int _level) {
            BuildLookup();

            float[] levels = lookupTable[_characterClass][_stats];

            if (_level > levels.Length) {
                return 0f;
            }

            return levels[_level - 1];
        }

        public int GetLevelsLength(CharacterClass _characterClass, Stats _stats) {
            float[] levels = lookupTable[_characterClass][_stats];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach(ProgressionCharacterClass progressionCharacterClass in characterClasses) {
                var statLookupTable = new Dictionary<Stats, float[]>();
                foreach (ProgressionStat stat in progressionCharacterClass.stats) {
                    statLookupTable[stat.stat] = stat.levels;
                }
                lookupTable[progressionCharacterClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        public class ProgressionCharacterClass 
        {
            public CharacterClass    characterClass = CharacterClass.Player;
            public ProgressionStat[] stats;

            public CharacterClass GetClass() {
                return characterClass;
            }
        }

        [System.Serializable]
        public class ProgressionStat 
        {
            public Stats   stat = Stats.Health;
            public float[] levels;
        }
    }
}