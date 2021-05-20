﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement 
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier { A, B, C, D }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if (sceneToLoad < 0) {
                Debug.LogError("Scene to load is not set");
                yield break;
            }

            DontDestroyOnLoad(this.gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>().GetComponent<SavingWrapper>();

            yield return fader.FadeOut(fadeDuration / 2);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.Load();

            Portal portalToSpawn = GetPortalToSpawn();
            UpdatePlayer(portalToSpawn);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeDuration / 2);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portalToSpawn)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portalToSpawn.spawnPoint.position);
            player.transform.rotation = portalToSpawn.spawnPoint.rotation;
        }

        private Portal GetPortalToSpawn()
        {
            foreach(Portal portal in GameObject.FindObjectsOfType<Portal>()) {
                if (portal != this && portal.destination == destination) {
                    return portal;
                }
            }

            return null;
        }
    }
}
