using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health        health     = null;
        [SerializeField] private RectTransform foreground = null;
        [SerializeField] private Canvas        canvas     = null;

        private void Update() {
            float fraction = health.GetFraction();
            canvas.enabled = fraction > 0 && fraction < 1f;
            foreground.localScale = new Vector2(fraction, foreground.localScale.y);
        }
    }
}
