using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [Header("EFFECTS")]
        [SerializeField] private AudioClip[] attackClips = null;

        [SerializeField] private Weapon initialWeapon = null;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        private GameObject newWeapon = null;
        private Weapon currentWeapon = null;

        private float timeSinceLastAttack = 0.0f;
        private Health target;

        void Start()
        {
            timeSinceLastAttack = Mathf.Infinity;

            EquipWeapon(initialWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target)
            {
                return;
            }           
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
                GetComponent<Animator>().SetTrigger("stopAttack");
            }
            else if (target)
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

            if (target.IsDead()) target = null;
        }

        public bool EquipWeapon(Weapon weapon)
        {
            if (weapon == null || weapon == currentWeapon) return false;

            if (newWeapon != null) Destroy(newWeapon);
            newWeapon = weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());

            weapon.ChangeAnimator(GetComponent<Animator>());

            currentWeapon = weapon;
            return true;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange();
        }

        void AttackBehaviour()
        {
            FixAttackRotation();
            if (timeSinceLastAttack >= currentWeapon.GetTimeBeetwenAttacks())
            {
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void FixAttackRotation()
        {
            Vector3 lookRotation = target.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), Time.deltaTime * 10f);
        }

        //animation event
        void Hit()
        {
            if (target) { target.TakeDamage(currentWeapon.GetDamage()); }

            if (GetComponent<AudioSource>() == null) { return; }
            GetComponent<AudioSource>().clip = attackClips[Random.Range(0, attackClips.Length)];
            GetComponent<AudioSource>().Play();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health healthToTest = combatTarget.GetComponent<Health>();
            return healthToTest != null && !healthToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
