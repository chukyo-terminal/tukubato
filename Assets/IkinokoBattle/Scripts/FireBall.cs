using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 5;
    public float lifeTime = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        var mob = other.GetComponent<MobStatus>();
        if (mob != null)
        {
            mob.Damage(damage);
        }

        Destroy(gameObject);
    }
}
