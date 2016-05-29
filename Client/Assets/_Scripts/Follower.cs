using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

    public Transform target;

    public float scanFrequency = 0.5f;
    private float lastScanFrequency = 0;
    public float stopFollowDistance = 2f;
    private NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToScan() && !isInRange())
        {
            agent.SetDestination(target.position);
        }
    }

    private bool isInRange()
    {
        return Vector3.Distance(target.position, transform.position) < stopFollowDistance;
    }

    private bool isReadyToScan()
    {
        return Time.time - lastScanFrequency > scanFrequency && target;
    }
}
