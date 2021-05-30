using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        private GameObject newWeapon = null;
        [SerializeField] private Weapon defaultWeapon = null;

        private float timeSinceLastAttack = 0.0f;
        private Health target;
        [HideInInspector] public Weapon currentWeapon = null;

        void Start()
        {
            timeSinceLastAttack = Mathf.Infinity;

            if (currentWeapon == null)
                EquipWeapon(defaultWeapon);
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
            if (target == null || currentWeapon == null) return false;
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
            if (target == null) { return; }

            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().clip = currentWeapon.attackSound;
                GetComponent<AudioSource>().Play();
            }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, this.gameObject);
                return;
            }

            target.TakeDamage(this.gameObject, currentWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
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

        public Health GetTarget() {
            return target;
        }

        void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            if (currentWeapon != null)
                return currentWeapon.name;
            return "Unarmed";
        }

        public void RestoreState(object state)
        {
            string weaponName = state as string;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
