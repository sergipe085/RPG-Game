using System;
using UnityEngine;

namespace RPG.Core 
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectsPrefab = null;
        
        private static bool hasSpawned = false;

        private void Awake() {
            if (!hasSpawned) {
                hasSpawned = true;
                SpawnPersistentObjects();
            }
        }

        private void SpawnPersistentObjects() {
            GameObject persistentObject = Instantiate(persistentObjectsPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}