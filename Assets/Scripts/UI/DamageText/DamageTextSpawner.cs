using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        private void Start() {
            Spawn(0.0f);
        }

        public void Spawn(float damage) {
            Instantiate(damageTextPrefab, transform);
        }
    }
}
