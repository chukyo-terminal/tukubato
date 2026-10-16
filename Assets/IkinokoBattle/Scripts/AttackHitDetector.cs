using UnityEngine;

public class AttackHitDetector : MonoBehaviour
{
    [SerializeField] private MobAttack mobAttack;

    private void OnTriggerEnter(Collider other)
    {
        mobAttack.OnHitAttack(other);
    }
}
