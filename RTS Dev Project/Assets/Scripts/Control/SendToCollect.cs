using UnityEngine;
using System.Collections;

public class SendToCollect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,20);
		foreach(Collider c in hitColliders){
			if(c.tag=="Ally"){
				UnitMovement u = c.GetComponentInParent<UnitMovement>();
				if(u.totalFood > 0){
					u.hasCollected = false;
					u.startMovingToCollect(u.targetToCollect);
				}
			}
		}
	}
}
