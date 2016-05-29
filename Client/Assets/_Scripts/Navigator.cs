using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    private Follower follower;
    
	void Awake () {
	    agent = GetComponent<NavMeshAgent>();
	    animator = GetComponent<Animator>();
	    follower = GetComponent<Follower>();

	}

    void Update()
    {
        animator.SetFloat("Distance", agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 position)
	{
	    agent.SetDestination(position);
	    follower.target = null;
	}
}
