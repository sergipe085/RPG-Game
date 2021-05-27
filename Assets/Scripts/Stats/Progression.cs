using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(CharacterClass _characterClass, Stats _stats, int _level) {
            foreach(ProgressionCharacterClass c in characterClasses) {
                if (c.characterClass != _characterClass) continue;
                
                foreach(ProgressionStat progressionStat in c.stats) {
                    if (_stats != progressionStat.stat) continue;
                    if (progressionStat.levels.Length < _level) continue;

                    return progressionStat.levels[_level];
                }
            }
            return 0f;
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