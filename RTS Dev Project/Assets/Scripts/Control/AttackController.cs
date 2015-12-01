using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {

    [SerializeField]
    private GameObject targetPrefab;
    private Identity identity;

    public GameObject attacking_target;
    public GameObject attacking_enemy;

    public double range;
    public double atkDmg;

    private Vector3 enemy_last_pos;
    private UnitMovement um;

    Animator animator;
	

	// Use this for initialization
	void Awake () {
		this.identity = this.gameObject.GetComponent<Identity> ();
		this.range = DataManager.Instance.unitDatas [identity.unitType].stats [Stat.Range];
		this.atkDmg = DataManager.Instance.unitDatas [identity.unitType].stats [Stat.Attack];
		um = gameObject.GetComponent<UnitMovement> ();
        animator = GetComponent<Animator>();
	}

	public void attack(GameObject enemy){
		Debug.Log (enemy.gameObject.layer);
		double r = this.range;

		this.attacking_enemy = enemy ;

		Vector3 myPos = this.gameObject.transform.position;
		Vector3 enemyPos = enemy.gameObject.transform.position;
		
		double d = RealDistance(enemy);

		if (!IsInRange(enemy)) {
			um.status = Status.running;
			Vector3 vec = enemyPos - myPos;
			vec = vec.normalized;
			double alpha = d - (r/3.0);
		
			vec.x *= (float)alpha;
			vec.z *= (float)alpha;
			Vector3 targetPos = myPos + vec;
			targetPos.y = 0;
		
			attacking_target = Instantiate (targetPrefab, targetPos, Quaternion.identity) as GameObject; //Instanciar prefab target
            attacking_target.transform.SetParent(GameController.Instance.targetsParent.transform);
			um.startMoving (attacking_target);
			this.enemy_last_pos = attacking_enemy.transform.position;
		} else {
			AttackManaging();
		}

	}

	// Update is called once per frame
	void Update () {
		if (this.attacking_enemy != null) { //check if position has changed, and follow if so

			if (IsInRange (attacking_enemy)) {
				AttackManaging ();
			}

			if (Vector3.Distance (enemy_last_pos, attacking_enemy.transform.position) > this.range) {
				this.um.status = Status.running;
				CancelInvoke ("DealDamage");
				this.attack (attacking_enemy);

			}
		} else {
			CancelInvoke ("DealDamage");
		}
	}

	//Makes the target lose health equal to this unit damage stat.
	private void DealDamage(){
		Debug.Log ("DealDamage");
		if (this.attacking_enemy != null && IsInRange(attacking_enemy)) {
			this.attacking_enemy.GetComponent<Health> ().loseHP ((int)this.atkDmg);
			Debug.Log ("Unit "+ this.identity.name +" dealt " + this.atkDmg + " damage.");
		}
	}

	//Returns a boolean indicating if the target is in attack range.
	private bool IsInRange(GameObject enemy){
		if (enemy != null) {
			float dist = RealDistance (enemy);
			return (dist < this.range);
		} else {
			return false;
		}
	}

	//Returns distance between this gameobject and another one, considering its size.
	private float RealDistance(GameObject enemy){
		if (enemy != null) {
			float dist;
			//Attacking a Building
			Vector3 boxSize = enemy.GetComponent<BoxCollider> ().size;
			float boxRadius = Mathf.Sqrt (boxSize.x * boxSize.x + boxSize.z * boxSize.z);

			dist = Vector3.Distance (this.gameObject.transform.position, enemy.transform.position) - boxRadius;
			return dist;
		} else {
			return (float)0.0;
		}
	}

	private void AttackManaging(){
		if (this.um.status == Status.attacking) {
			//Already attacking and already invoked routine.

		} else {
			this.um.status = Status.attacking;
			InvokeRepeating ("DealDamage", 1, 1);
			animator.SetBool ("attack",true);

		}

	}

}

