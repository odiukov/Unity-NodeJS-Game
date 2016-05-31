using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {

	public float attackDistance = 1;
	public float attackRate = 2;
	float lastAttackTime = 0;
	Targeter targeter;

	void Start () {
		targeter = GetComponent<Targeter> ();	
	}
	
	void Update () {
		if (isReadyToAttack() && targeter.IsInRange (attackDistance)) {
			Debug.Log("attacking " + targeter.target.name);
			var targetId = targeter.target.GetComponent<NetworkEntity> ().id;
			Network.Attack (targetId);
			lastAttackTime = Time.time;
		}
	}

	private bool isReadyToAttack()
	{
		return Time.time - lastAttackTime > attackRate && targeter.target;
	}
}
