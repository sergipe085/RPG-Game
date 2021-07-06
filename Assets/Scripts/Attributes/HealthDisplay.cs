﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health health = null;

        private void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealth(), health.GetMaxHealth());
        }
    }
}   
