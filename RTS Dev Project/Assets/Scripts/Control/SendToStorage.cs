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
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,10);
		foreach(Collider c in hitColliders){
			if(c.tag=="Ally"){
				CollectResources collect = c.gameObject.GetComponentInParent<CollectResources>();
				if(collect.goingToCollect == true){
					if(collect.targetToCollect == null || collect.targetToCollect == gameObject.transform){
						DestroyOnExpend d = gameObject.GetComponent<DestroyOnExpend>();
						if(collect.hasCollected == false){
							d.amount = d.amount - 10;
							collect.totalFood = d.amount;
							collect.targetToCollect = gameObject.transform;
                            collect.hasCollected = true;
                            collect.goingToCollect = false;
							collect.startMovingToStorage(t.transform);
							
						}
					}
				}
			}
		}
	}
	
	
}
