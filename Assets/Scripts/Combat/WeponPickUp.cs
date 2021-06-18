using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            if (fighter.EquipWeapon(weapon))
            {
                StartCoroutine(HideForSeconds(3));
            }
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
                Pickup(controller.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
    }
}
