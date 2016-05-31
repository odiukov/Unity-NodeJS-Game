using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

	private float health;
	public float maxHealth = 100;

	void Start () {
		health = maxHealth;
	}

	public void GetHit(float damage){
		if (health - damage <= 0) {
			health = 0;
			Debug.Log ("Dead");
		} else
			health -= damage;
	}
}
