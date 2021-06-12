using UnityEngine;
using RPG.Core;
using RPG.Combat;
using RPG.Resources;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab = null;
    public AnimatorOverrideController weaponAnimator = null;
    public Projectile projectile = null;
    public AudioClip attackSound = null;
    public float playerSpeed = 0.0f;
    public float weaponRange = 0.0f;
    public float weaponDamage = 0.0f;
    public float percentageBonus = 0.0f;
    public float timeBetweenAttacks = 0.0f;
    public bool isRightHanded = true;

    public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        if (weaponPrefab != null)
        {
            return Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));
        }
        return null;
    }

    Transform GetTransform(Transform rightHand, Transform leftHand)
    {
        Transform handTransform;
        if (isRightHanded) handTransform = rightHand;
        else handTransform = leftHand;
        return handTransform;
    }

    public bool HasProjectile()
    {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damage)
    {
        Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
        projectileInstance.SetTarget(target, instigator, damage);
    }

    public void ChangeAnimator(Animator animator)
    {
        if (weaponAnimator != null) animator.runtimeAnimatorController = weaponAnimator;
    }

    public float GetDamage()
    {
        return weaponDamage;
    }

    public float GetPercentageBonus() {
        return percentageBonus;
    }

    public float GetRange()
    {
        return weaponRange;
    }

    public float GetTimeBeetwenAttacks()
    {
        return timeBetweenAttacks;
    }
}
