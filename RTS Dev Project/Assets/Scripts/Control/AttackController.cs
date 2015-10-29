using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	public enum  attacking;
	GameObject attacking_target;
	public double range;
	// Use this for initialization
	void Start () {
		attacking = false;
		range = 30.0;
	}

	void attack(GameObject enemy){
		attacking = true;
		attacking_target = enemy;
	}

	// Update is called once per frame
	void Update () {
		if (attacking) {

		}
	}
}
