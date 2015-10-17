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
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,3);
		foreach(Collider c in hitColliders){
			if(c.tag=="Ally"){
				UnitMovement u = c.GetComponentInParent<UnitMovement>();
				u.startMovingAfterCollect(t.transform);

			}
		}
		
	}
	
	
}
