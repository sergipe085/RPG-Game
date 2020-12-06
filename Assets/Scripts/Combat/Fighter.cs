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

        [SerializeField] private float weaponRange = 2.0f;
        [SerializeField] private float timeBetweenAttacks = 1.0f;
        [SerializeField] private float weaponDamage = 5.0f;
        [SerializeField] private Weapon sword = null;
        [SerializeField] private Transform handTransform = null;

        private float timeSinceLastAttack = 0.0f;
        private Health target;

        void Start()
        {
            timeSinceLastAttack = timeBetweenAttacks;

            SpawnWeapon(sword);
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

        void SpawnWeapon(Weapon weapon)
        {
            weapon.Spawn(handTransform, GetComponent<Animator>());
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        void AttackBehaviour()
        {
            FixAttackRotation();
            if (timeSinceLastAttack >= timeBetweenAttacks)
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
            if (target) { target.TakeDamage(weaponDamage); }

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
