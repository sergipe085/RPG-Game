using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text text = null;

        public void SetTextValue(string value) {
            text.text = value;
        }

        public void DestroyText() {
            Destroy(gameObject);
        }
    }
}
