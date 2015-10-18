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
				CollectResources collect = c.GetComponentInParent<CollectResources>();
				if(collect.totalFood > 0){
					collect.hasCollected = false;
					collect.startMovingToCollect(collect.targetToCollect);
				} else {
					collect.hasCollected = false;
				}

			}
		}
	}
}
