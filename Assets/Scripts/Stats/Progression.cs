using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetValue(CharacterClass _characterClass, Stats _stats, int _level) {
            ProgressionCharacterClass characterClass = null;
            foreach(ProgressionCharacterClass c in characterClasses) {
                if (c.GetClass() == _characterClass) {
                    characterClass = c;
                    break;
                }
            }
            if (characterClass == null) return 1f;
            
            return characterClass.GetValue(_stats, _level);
        }

        [System.Serializable]
        public class ProgressionCharacterClass 
        {
            [SerializeField] private CharacterClass    characterClass = CharacterClass.Player;
            [SerializeField] private ProgressionStat[] stats;

            public CharacterClass GetClass() {
                return characterClass;
            }

            // public float GetHealth(int _level) {
            //     if (health.Length == 0) return 1f;

            //     int level = Mathf.Clamp(_level - 1, 0, health.Length - 1);
            //     return health[level];
            // }

            public float GetValue(Stats _stats, int _level) {
                ProgressionStat progressionStat = null;
                foreach (ProgressionStat p in stats) {
                    if (p.stat == _stats) {
                        progressionStat = p;
                    }
                }

                if (progressionStat == null) return 1f;

                return progressionStat.levels[_level];
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