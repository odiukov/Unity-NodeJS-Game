using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

	private float health;
	public float maxHealth = 100;
	Animator animator;

	public bool IsDead {
		get { return health <= 0; }
	}

	void Start () {
		animator = GetComponent<Animator> ();
		health = maxHealth;
	}

	public void GetHit(float damage){
		health -= damage;
		if (IsDead) {
			animator.SetTrigger ("Dead");
			Debug.Log ("Dead");
		}
	}
}
