using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weapon = null;
        [SerializeField] private float healthToRestore = 0.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null) {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (healthToRestore > 0) {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(3));
        }

        private IEnumerator HideForSeconds(float time) {
            HidePickup();
            yield return new WaitForSeconds(time);
            ShowPickup();
        }

        private void ShowPickup()
        {
            GetComponent<Collider>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        private void HidePickup()
        {
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0)) {
                Pickup(controller.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
    }
}
