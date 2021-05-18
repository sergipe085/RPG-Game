using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile) {
            print("Saving to " + GetPathFromSaveFile(saveFile));

            using (FileStream stream = File.Open(GetPathFromSaveFile(saveFile), FileMode.Create)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string saveFile) {
            print("Loading from " + GetPathFromSaveFile(saveFile));
            using (FileStream stream = File.Open(GetPathFromSaveFile(saveFile), FileMode.Open)) {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        private void RestoreState(object state) {
            Dictionary<string, object> stateDict = state as Dictionary<string, object>;
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) {
                saveable.RestoreState(stateDict[saveable.GetUID()]);
            }
        }

        private object CaptureState() {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) {
                state[saveable.GetUID()] = saveable.CaptureState();
            }
            return state;
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}