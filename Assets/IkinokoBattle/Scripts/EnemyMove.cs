using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] private PlayerController _playerController;
    [SerializeField] private LayerMask raycastRayerMask;
    private NavMeshAgent _agent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    private EnemyStatus _status;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _status = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    //void Update()
    //{
        //_agent.destination = _playerController.transform.position;
    //}

    public void OnDetectObject(Collider collider)
    {
        if(!_status.IsMovable)
        {
            _agent.isStopped = true;
            return;
        }
        
        if (collider.CompareTag("Player"))
        {
            var positionDiff = collider.transform.position - transform.position;
            var distance = positionDiff.magnitude;
            var direction = positionDiff.normalized;
            var hitcount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, raycastRayerMask);
            Debug.Log("hitcount: " + hitcount);
            if(hitcount == 0)
            {
                _agent.isStopped = false;
                _agent.destination = collider.transform.position;
            }
        }
        else
        {
            _agent.isStopped = true;
        }
    }
}
