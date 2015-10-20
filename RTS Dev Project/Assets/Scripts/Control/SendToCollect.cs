using UnityEngine;
using System.Collections;

public class SendToCollect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,15);
		foreach(Collider c in hitColliders){
			if(c.tag=="Ally"){
				CollectResources collect = c.GetComponentInParent<CollectResources>();
				if (collect == null){
					// Doesn't have component, so doesn't collect
					return;
				} else if (!collect.goingToCollect)
                {
                    collect.goingToCollect = true;
                    collect.hasCollected = false;
                    if (collect.targetToCollect!=null){
					    collect.startMovingToCollect(collect.targetToCollect);
				    } else {
                        AI.Instance.reassignResourceToCivilian(c.gameObject);
				    }
                }
                

			}
		}
	}
}
