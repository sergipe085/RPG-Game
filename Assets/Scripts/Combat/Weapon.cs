using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab = null;
    public AnimatorOverrideController weaponAnimator = null;

    public void Spawn(Transform transform, Animator animator)
    {
        Instantiate(weaponPrefab, transform);
        animator.runtimeAnimatorController = weaponAnimator;
    }
}
