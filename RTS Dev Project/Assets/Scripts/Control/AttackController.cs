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

	public void attack(GameObject enemy){
		attacking_target = enemy;

		//Crear nou metode moveUnit
		//Crear interficie de atac!!!!!!!!!!!!!

		
		Vector3 myPos = this.gameObject.transform.position;
		Vector3 enemyPos = enemy.gameObject.transform.position;
		
		double d = Vector3.Distance(myPos,enemyPos);
		
		Vector3 vec = enemyPos - myPos;
		
		vec = vec.normalized;
		
		double r = this.range;
		
		Debug.Log(r);
		
		double alpha =  d-(r/2.0);
		
		
		vec.x *= (float) alpha;
		vec.z *= (float) alpha;
		
		Vector3 targetPos = myPos + vec;
		
		
		GameObject target = Instantiate(targetPrefab, targetPos, Quaternion.identity) as GameObject;
		moveUnits(target);

	}

	// Update is called once per frame
	void Update () {
		if (um.status == Status.attacking) {

		}
	}
}
