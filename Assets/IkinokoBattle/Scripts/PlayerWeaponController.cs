using UnityEngine;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Animator animator;

    [SerializeField] private WeaponData currentWeapon;
    private GameObject equippedWeaponModel;

    private MobAttack mobAttack;
    private MobStatus mobStatus;

    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float attackCooldown = 0.5f;
    private GameObject currentHit;

    private void Start()
    {
        mobAttack = GetComponent<MobAttack>();
        mobStatus = GetComponent<MobStatus>();
        mobAttack.OnAttackStartEvent += OnAttackStart;
        mobAttack.OnAttackFinishedEvent += OnAttackFinished;
    }


    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        int weaponLevel = OwnedItemsData.Instance.GetItem(newWeapon.weaponType)?.Number ?? 1;
        weaponLevel = Mathf.Clamp(weaponLevel, 1, newWeapon.levelDamage.Length);


        // 古い武器モデルを消す
        if (equippedWeaponModel != null)
            Destroy(equippedWeaponModel);

        // 新しい武器モデルを生成
        equippedWeaponModel = Instantiate(newWeapon.weaponPrefab, weaponHolder);

        // モーション切り替え
        if (newWeapon.attackAnimation != null || newWeapon.idleAnimation)
        {
            animator.runtimeAnimatorController =
                RuntimeAnimatorControllerFromClip(newWeapon.attackAnimation, newWeapon.idleAnimation);
        }


    }

    public void OnAttackStart()
    {
        int weaponLevel = OwnedItemsData.Instance.GetItem(currentWeapon.weaponType)?.Number ?? 1;
        weaponLevel = Mathf.Clamp(weaponLevel, 1, currentWeapon.levelDamage.Length);

        if (currentHit != null)
            Destroy(currentHit);
        // 攻撃判定を生成
        Vector3 spawnPos =
        attackOrigin.position +
        attackOrigin.forward * currentWeapon.hitOffset.z +
        attackOrigin.up * currentWeapon.hitOffset.y +
        attackOrigin.right * currentWeapon.hitOffset.x;

        currentHit = Instantiate(currentWeapon.hitDetectorPrefab, spawnPos, attackOrigin.rotation);

        int dmg = currentWeapon.levelDamage[weaponLevel - 1];
        currentHit.GetComponent<HitDetector>().Setup(dmg);
        Debug.Log("Current Weapon: " + currentWeapon.weaponName + " Damage: " + currentWeapon.damage);
        
    }

    public void OnAttackFinished()
    {
        Debug.Log("attackfinish");
        Debug.Log("currentHit = " + currentHit);
        if (currentHit != null)
        {
            Destroy(currentHit);
            currentHit = null;
        }
        mobAttack.StartCoroutine(CooldownCoroutine());
    }

    private RuntimeAnimatorController RuntimeAnimatorControllerFromClip(AnimationClip attackclip, AnimationClip idleclip)
    {
        var controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
        controller["Attack1_custom"] = attackclip; // Attack を差し替える
        controller["1HCombatIdle"] = idleclip;
        return controller;
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        mobStatus.GoToNormalStateIfPossible();
    }
}