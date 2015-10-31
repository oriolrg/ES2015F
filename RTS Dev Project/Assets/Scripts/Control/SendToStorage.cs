﻿using UnityEngine;
using System.Collections;

public class SendToStorage : MonoBehaviour {
	
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,10);
		foreach(Collider c in hitColliders){
            if (c.tag == "Ally" || c.tag == "Enemy")
            {
                CollectResources collect = c.gameObject.GetComponentInParent<CollectResources>();
		        if(collect!=null && collect.goingToCollect == true){
                    if (collect.targetToCollect == gameObject){
                        DestroyOnExpend d = gameObject.GetComponent<DestroyOnExpend>();
				        if(collect.hasCollected != true){
					        d.amount = d.amount - 10;
                            collect.resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), this.tag);
                            collect.quantityCollected = 10;                         
                            collect.startMovingToStorage(AI.Instance.getClosestTownCenter(c.gameObject));
				        }
			        }
		        }
            }
            
		
		}
	}
	
	
}
