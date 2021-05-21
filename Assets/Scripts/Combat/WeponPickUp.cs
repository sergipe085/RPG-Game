using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Fighter>().EquipWeapon(weapon))
                {
                    StartCoroutine(HideForSeconds(3));
                }
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
    }
}
