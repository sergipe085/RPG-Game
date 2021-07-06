using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter = null;

        private void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            Health target = fighter.GetTarget();
            if (target) {
                GetComponent<Text>().text = String.Format("{0:0}/{1:0}", target.GetHealth(), target.GetMaxHealth());
            }
            else {
                GetComponent<Text>().text = "N/A";
            }
        }
    }
}   
