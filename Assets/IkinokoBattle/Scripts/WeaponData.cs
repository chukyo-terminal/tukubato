using UnityEngine;

[CreateAssetMenu(menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public GameObject weaponPrefab;      // 手に持つモデル
    public GameObject hitDetectorPrefab; // 攻撃判定プレハブ

    public int damage;
    public float attackCooldown;
    public float attackRange;

    public AnimationClip attackAnimation; // ← モーションも武器ごとに変える
    public AnimationClip idleAnimation;

    public Vector3 hitOffset = new Vector3(0, 1, 1.5f);
    //public AnimatorOverrideController overrideController;
    //animator.runtimeAnimatorController = newWeapon.overrideController;

}
