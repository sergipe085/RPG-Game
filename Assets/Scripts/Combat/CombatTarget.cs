using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController controller)
        {
            if (!controller.fighter.CanAttack(this.gameObject)) return false;

            if (Input.GetMouseButton(0)) {
                controller.fighter.Attack(this.gameObject);
            }
            return true;
        }
    }
}
