using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

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
                GetComponent<Text>().text = String.Format("{0:0}%", target.GetPercentage());
            }
            else {
                GetComponent<Text>().text = "N/A";
            }
        }
    }
}   
