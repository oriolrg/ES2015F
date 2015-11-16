using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	GameObject attacking_target;
	public double range;
	UnitMovement um;
    Animator animator;
    
    // Use this for initialization
	void Start () {
		range = 30.0;
		um = gameObject.GetComponent<UnitMovement> ();
        animator = GetComponent<Animator>();
	}

	public void attack(GameObject enemy)
    {
        if( animator != null )
            animator.SetBool("attack", true);

        attacking_target = enemy;
	}

	// Update is called once per frame
	void Update () {
		if (um.status == Status.attacking) {

		}
	}
}
