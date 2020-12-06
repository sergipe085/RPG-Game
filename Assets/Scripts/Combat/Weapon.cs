using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab = null;
    public AnimatorOverrideController weaponAnimator = null;
    public float weaponRange = 0.0f;
    public float weaponDamage = 0.0f;
    public float timeBetweenAttacks = 0.0f;
    public bool isRightHanded = true;

    public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        if (weaponPrefab != null)
        {
            if (isRightHanded) return Instantiate(weaponPrefab, rightHand);
            else return Instantiate(weaponPrefab, leftHand);
        }
        return null;
    }

    public void ChangeAnimator(Animator animator)
    {
        if (weaponAnimator != null) animator.runtimeAnimatorController = weaponAnimator;
    }

    public float GetDamage()
    {
        return weaponDamage;
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
