using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MobStatus))]
public class RangeAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject fireballPrefab;

    private MobStatus _status;

    public System.Action OnAttackStartEvent;
    public System.Action OnAttackFinishedEvent;

    private void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    public void AttackIfPossible()
    {
        if (!_status.IsAttackable) return;
        _status.GoToAttackStateIfPossible();
    }

    public void OnAttackRangeEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            AttackIfPossible();
        }
    }

    // AnimationEvent ‚ÅŒÄ‚Ô
    public void OnAttackStart()
    {
        ShootFireball();
        OnAttackStartEvent?.Invoke();
    }

    private void ShootFireball()
    {
        Instantiate(fireballPrefab, shootPoint.position, shootPoint.rotation);
    }

    // AnimationEvent ‚ÅŒÄ‚Ô
    public void OnAttackFinished()
    {
        StartCoroutine(CooldownCoroutine());
        OnAttackFinishedEvent?.Invoke();
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _status.GoToNormalStateIfPossible();
    }
}
