using UnityEngine;
using System.Collections;

public class SendToStorage : MonoBehaviour {
	
	GameObject t;
	
	// Use this for initialization
	void Start () {
		t = GameObject.FindGameObjectWithTag("StorageFood");
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,15);
		foreach(Collider c in hitColliders){
			if(c.tag=="Ally"){
				UnitMovement u = c.GetComponentInParent<UnitMovement>();
				DestroyOnExpend d = gameObject.GetComponent<DestroyOnExpend>();
				if(u.hasCollected == false){
					d.amount = d.amount - 10;
					u.totalFood = d.amount;
					u.targetToCollect = gameObject.transform;
					u.startMovingToStorage(t.transform);
				}
			}
		}
	}
	
	
}
