using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private string saveName = "save01";

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                GetComponent<SavingSystem>().Save(saveName);
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                GetComponent<SavingSystem>().Load(saveName);
            }
        }
    }
}