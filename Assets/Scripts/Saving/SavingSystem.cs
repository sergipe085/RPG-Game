using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile) {
            print("Saving to " + GetPathFromSaveFile(saveFile));

            using (FileStream stream = File.Open(GetPathFromSaveFile(saveFile), FileMode.Create)) {
                Transform playerTransform = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, playerTransform.position);
            }
        }

        public void Load(string saveFile) {
            print("Loading from " + GetPathFromSaveFile(saveFile));
            using (FileStream stream = File.Open(GetPathFromSaveFile(saveFile), FileMode.Open)) {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                
                Vector3 pos = DeserializeVector(buffer);
                GetPlayerTransform().position = pos;
            }
        }

        private Transform GetPlayerTransform() {
            return GameObject.FindWithTag("Player").transform;
        }

        private byte[] SerializeVector(Vector3 vector) {
            byte[] vectorBytes = new byte[sizeof(float) * 3];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, sizeof(float));
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, sizeof(float) * 2);
            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer) {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, sizeof(float));
            result.z = BitConverter.ToSingle(buffer, sizeof(float));
            return result;
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}