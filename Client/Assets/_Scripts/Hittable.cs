using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

	public float health = 100;
	Animator animator;
	public float respawnTime = 5;

	public bool IsDead {
		get { return health <= 0; }
	}

	void Start () {
		animator = GetComponent<Animator> ();
	}

	public void GetHit(float damage){
		health -= damage;
		if (IsDead) {
			animator.SetTrigger ("Dead");
			Invoke ("Spawn", respawnTime);
		}
	}

	void Spawn(){
		Debug.Log ("Spawn the player");
		transform.position = Vector3.zero;
		health = 100;
		animator.SetTrigger ("Spawn");
	}
}
