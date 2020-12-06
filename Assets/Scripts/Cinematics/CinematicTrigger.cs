using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool activated = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !activated)
            {
                activated = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
