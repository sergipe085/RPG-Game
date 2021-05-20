using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private string saveName = "save01";

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(saveName);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(saveName);
        }
    }
}