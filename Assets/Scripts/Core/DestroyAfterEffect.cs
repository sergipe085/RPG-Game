using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;

        void Update()
        {
            GameObject a = targetToDestroy ? targetToDestroy : gameObject;
            if (!GetComponent<ParticleSystem>().IsAlive()) { Destroy(a); }
        }
    }
}
