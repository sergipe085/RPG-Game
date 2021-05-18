using UnityEditor;
using UnityEngine;

namespace RPG.Saving 
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string UID = "";

        public string GetUID() {
            return UID;
        }

        public object CaptureState() {
            print("Getting state from " + GetUID());
            return null;
        }

        public void RestoreState(object state) {
            print("Restoring state from " + GetUID());
        }

        #if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject) || string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("UID");

            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
        #endif
    }
}