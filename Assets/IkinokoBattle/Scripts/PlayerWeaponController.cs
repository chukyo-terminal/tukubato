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
    [SerializeField] private AudioSource swing;
    private GameObject currentHit;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private CharacterController controller;


    private void Start()
    {
        mobAttack = GetComponent<MobAttack>();
        mobStatus = GetComponent<MobStatus>();
        controller = GetComponent<CharacterController>();
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

        if(swing != null)
        {
            swing.pitch = Random.Range(0.7f, 1.3f);
            swing.Play();
        }

        int dmg = currentWeapon.levelDamage[weaponLevel - 1];
        currentHit.GetComponent<HitDetector>().Setup(dmg);

        if (currentWeapon.isDashAttack)
        {
            isDashing = true;
            dashTimer = currentWeapon.dashTime;
        }

        Debug.Log("Current Weapon: " + currentWeapon.weaponName + " Damage: " + currentWeapon.damage);
        
    }

    private void Update()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            Vector3 dir = transform.forward * (currentWeapon.dashDistance / currentWeapon.dashTime);
            controller.Move(dir * Time.deltaTime);

            if (currentHit != null)
            {
                currentHit.transform.position =
                    attackOrigin.position +
                    attackOrigin.forward * currentWeapon.hitOffset.z +
                    attackOrigin.up * currentWeapon.hitOffset.y +
                    attackOrigin.right * currentWeapon.hitOffset.x;

                currentHit.transform.rotation = attackOrigin.rotation;
            }

            if (dashTimer <= 0f)
            {
                isDashing = false;
                if (currentHit != null)
                {
                    Destroy(currentHit);
                    currentHit = null;
                }
            }
        }
    }


    public void OnAttackFinished()
    {
        Debug.Log("attackfinish");
        Debug.Log("currentHit = " + currentHit);
        if (!currentWeapon.isDashAttack)
        {
            if (currentHit != null)
            {
                Destroy(currentHit);
                currentHit = null;
            }
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