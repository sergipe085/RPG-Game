using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private string saveName = "save01";

        private IEnumerator Start() {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(saveName);
            yield return fader.FadeIn(1.0f);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.Delete)) {
                Delete();
            }
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(saveName);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(saveName);
        }

        public void Delete() {
            GetComponent<SavingSystem>().Delete(saveName);
        }

        private void OnApplicationQuit() {
            Save();
        }
    }
}