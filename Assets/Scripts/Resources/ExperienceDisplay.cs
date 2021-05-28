using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceDisplay : MonoBehaviour
{
    private Experience experience = null;

    private void Awake()
    {
        experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }

    private void Update()
    {
        GetComponent<Text>().text = String.Format("{0}", experience.GetExperience().ToString());
    }
}
