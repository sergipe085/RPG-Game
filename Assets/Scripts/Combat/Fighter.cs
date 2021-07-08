using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        private Weapon newWeapon = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;

        private float timeSinceLastAttack = 0.0f;
        private Health target;
        [HideInInspector] public WeaponConfig currentWeaponConfig = null;
        Weapon currentWeapon = null;

        void Start()
        {
            timeSinceLastAttack = Mathf.Infinity;

            if (currentWeaponConfig == null)
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

        public bool EquipWeapon(WeaponConfig weapon)
        {
            if (weapon == null || weapon == currentWeaponConfig) return false;

            if (newWeapon != null) Destroy(newWeapon.gameObject);
            newWeapon = weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());

            weapon.ChangeAnimator(GetComponent<Animator>());

            currentWeaponConfig = weapon;
            currentWeapon = newWeapon;
            return true;
        }

        private bool GetIsInRange()
        {
            if (target == null || currentWeaponConfig == null) return false;
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.GetRange();
        }

        void AttackBehaviour()
        {
            FixAttackRotation();
            if (timeSinceLastAttack >= currentWeaponConfig.GetTimeBeetwenAttacks())
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

            float damage = GetComponent<BaseStats>().GetStat(Stats.Stats.Damage);

            currentWeapon?.OnHit();

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, this.gameObject, damage);
                return;
            }

            target.TakeDamage(this.gameObject, damage);
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

        public IEnumerable<float> GetAdditiveModifier(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage) {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage) {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
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
            if (currentWeaponConfig != null)
                return currentWeaponConfig.name;
            return "Unarmed";
        }

        public void RestoreState(object state)
        {
            string weaponName = state as string;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
