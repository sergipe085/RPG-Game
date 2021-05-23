using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass _characterClass, int _level) {
            ProgressionCharacterClass characterClass = null;
            foreach(ProgressionCharacterClass c in characterClasses) {
                if (c.GetClass() == _characterClass) {
                    characterClass = c;
                    break;
                }
            }
            if (characterClass == null) return 1f;
            
            return characterClass.GetHealth(_level);
        }

        [System.Serializable]
        public class ProgressionCharacterClass 
        {
            [SerializeField] private CharacterClass characterClass = CharacterClass.Player;
            [SerializeField] private float[]        health;

            public CharacterClass GetClass() {
                return characterClass;
            }

            public float GetHealth(int _level) {
                if (health.Length == 0) return 1f;

                int level = Mathf.Clamp(_level - 1, 0, health.Length - 1);
                return health[level];
            }
        }
    }
}