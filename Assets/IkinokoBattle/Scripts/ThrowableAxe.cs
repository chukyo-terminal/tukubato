using UnityEngine;

public class ThrowableAxe : MonoBehaviour
{
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private float lifeTime = 1.0f;
    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Throw(Vector3 direction)
    {
        rb.isKinematic = false;

        rb.AddForce(direction * throwForce, ForceMode.Impulse);

        Vector3 spinAxe = Vector3.Cross(direction, Vector3.right); 
        rb.AddTorque(spinAxe * 20f, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }


    public void OnTriggerEnter(Collider other)
    {
        HitAxe(other);
    }

    public void HitAxe(Collider collider)
    {
        Debug.Log("HitAxe:" + collider.name);

        var targetMob = collider.GetComponent<MobStatus>();

        if (null == targetMob) return;
        targetMob.Damage(2);
    }
}

