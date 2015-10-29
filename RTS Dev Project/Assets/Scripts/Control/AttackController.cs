using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	GameObject attacking_target;
	public double range;
	UnitMovement um;
	// Use this for initialization
	void Start () {
		range = 30.0;
		um = gameObject.GetComponent<UnitMovement> ();
	}

	void attack(GameObject enemy){
		attacking_target = enemy;
	}

	// Update is called once per frame
	void Update () {
		if (um.status == Status.attacking) {

		}
	}
}
