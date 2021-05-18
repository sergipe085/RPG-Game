using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
            return new SerializebleVector3(transform.position);
        }

        public void RestoreState(object state) {
            SerializebleVector3 vector = state as SerializebleVector3;
            GetComponent<NavMeshAgent>().Warp(vector.ToVector());
            GetComponent<ActionScheduler>().CancelCurrentAction();
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