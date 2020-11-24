using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2.0f;
        Transform target;

        private void Update()
        {
            if (!target) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else if (target)
            {
                GetComponent<Mover>().Stop();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
