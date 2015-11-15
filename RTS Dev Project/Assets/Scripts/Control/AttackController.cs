using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {

	[SerializeField]
	private GameObject targetPrefab;

	public GameObject attacking_target;
	public GameObject attacking_enemy;
	public double range;

	private Vector3 enemy_last_pos;
	private UnitMovement um;
	// Use this for initialization
	void Start () {
		if (range == 0 || range == null) {
			range = 5.0;
		}
		um = gameObject.GetComponent<UnitMovement> ();
	}

	public void attack(GameObject enemy){
		this.attacking_enemy = enemy ;

		Vector3 myPos = this.gameObject.transform.position;
		Vector3 enemyPos = enemy.gameObject.transform.position;
		
		double d = Vector3.Distance(myPos,enemyPos);

		if (d >= range) {
			um.status = Status.running;
		
			Vector3 vec = enemyPos - myPos;
		
			vec = vec.normalized;
		
			double r = this.range;
		
			double alpha = d - (r / 2.0);
		
		
			Debug.Log ("Move to enemy!");

			vec.x *= (float)alpha;
			vec.z *= (float)alpha;
		
			Vector3 targetPos = myPos + vec;
		
		
			attacking_target = Instantiate (targetPrefab, targetPos, Quaternion.identity) as GameObject; //Instanciar prefab target
			um.startMoving (attacking_target);
			this.enemy_last_pos = attacking_enemy.transform.position;
		} else {
			um.status = Status.attacking;
			Debug.Log("Die madafaka!");
		}

	}

	// Update is called once per frame
	void Update () {
		if (this.attacking_enemy != null) { //check if position has changed, and follow if so

			Vector3 enemyPos = attacking_enemy.transform.position;

			if(Vector3.Distance(enemy_last_pos,enemyPos) > this.range/2.0){
				this.attack (attacking_enemy);
			}




		}
	}
}
