using UnityEngine;
using System.Collections;

public class SendToStorage : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        GameObject c = collision.gameObject;
        //print(c.name);
        if (c.tag == "Ally" || c.tag == "Enemy")
        {
            CollectResources collect = c.gameObject.GetComponentInParent<CollectResources>();
            if (collect != null && collect.goingToCollect == true)
            {
                if (collect.targetToCollect == gameObject)
                {
                    DestroyOnExpend d = gameObject.GetComponent<DestroyOnExpend>();
                    if (collect.hasCollected != true)
                    {
                        d.amount = d.amount - 10;
                        collect.resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), this.tag);
                        collect.quantityCollected = 10;
                        collect.startMovingToStorage(AI.Instance.getClosestTownCenter(c.gameObject));
                    }
                }
            }
        }
    }
	
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,5);
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
