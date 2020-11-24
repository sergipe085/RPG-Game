using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
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
                GetComponent<Mover>().Cancel();
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        //animation event
        void Hit()
        {
            
        }
    }
}
