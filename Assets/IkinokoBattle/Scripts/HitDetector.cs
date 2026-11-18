using UnityEngine;

public class HitDetector : MonoBehaviour
{
    private int damage;
    [SerializeField] private LayerMask targetLayers;
    public void Setup(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            return;

        var mob = other.GetComponentInParent<MobStatus>();
        if (mob != null)
        {
            mob.Damage(damage);
        }
    }
}
