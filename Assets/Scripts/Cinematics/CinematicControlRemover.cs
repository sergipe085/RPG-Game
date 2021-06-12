using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;
using System;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject Player;

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector nonsense)
        {
            Player.GetComponent<ActionScheduler>().CancelCurrentAction();
            Player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector nonsense)
        {
            Player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
