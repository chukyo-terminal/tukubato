using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private NavMeshAgent _agent;

    public int pointValue = 2;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
    }

    protected override void OnDie()
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            GameManager.instance.AddPoint(pointValue);
        }
        
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
        PlayerEvents.OnEnemyKilled?.Invoke();
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
