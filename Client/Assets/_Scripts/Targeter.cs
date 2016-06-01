using UnityEngine;
using System.Collections;

public class Targeter : MonoBehaviour {

	public Transform target;

	public bool IsInRange(float stopFollowDistance)
	{
		return Vector3.Distance(transform.position, target.position) < stopFollowDistance;
	}

	public void ResetTarget(){
		target = null;
	}
}
