using UnityEngine;
using System.Collections;

public class NavigatePosition : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;

    
	void Start () {
	    agent = GetComponent<NavMeshAgent>();
	    animator = GetComponent<Animator>();
	}

    void Update()
    {
        animator.SetFloat("Distance", agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 position)
	{
	    agent.SetDestination(position);
	}
}
