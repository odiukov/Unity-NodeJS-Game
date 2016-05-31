using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

	public Targeter targeter;

    public float scanFrequency = 0.5f;
    private float lastScanFrequency = 0;
    public float stopFollowDistance = 2f;
    private NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		targeter = GetComponent<Targeter> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (isReadyToScan() && !targeter.IsInRange(stopFollowDistance))
        {
			agent.SetDestination(targeter.target.position);
        }
    }

    private bool isReadyToScan()
    {
		return Time.time - lastScanFrequency > scanFrequency && targeter.target;
    }
}
