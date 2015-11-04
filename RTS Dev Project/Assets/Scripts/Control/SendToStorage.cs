using UnityEngine;
using System.Collections;

public class SendToStorage : MonoBehaviour
{

    CollectResources r;
    GameObject choque;

    public void moveToStorageAfterCut()
    {
        CancelInvoke("moveToStorageAfterCut");
        choque.GetComponent<Animator>().SetBool("cut", false);
        r.startMovingToStorage(AI.Instance.getClosestTownCenter(choque.gameObject));
    }
	
	void Start()
    {
        InvokeRepeating("check", 1, 1);
    }
	// Update is called once per frame
	void check () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,5);
		foreach(Collider c in hitColliders){
            if (c.tag == "Ally" || c.tag == "Enemy")
            {
                CollectResources collect = c.gameObject.GetComponentInParent<CollectResources>();
		        if(collect!=null && collect.goingToCollect == true){
                    if (collect.targetToCollect == gameObject){
                        DestroyOnExpend d = gameObject.GetComponent<DestroyOnExpend>();
				        if(collect.hasCollected != true){
					        d.amount--;
                            collect.resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), this.tag);
                            collect.quantityCollected++;
                            c.GetComponent<Animator>().SetBool("cut", true);
                            choque = c.gameObject;
                            r = collect;
                            //moveToStorageAfterCut();
                            Invoke("moveToStorageAfterCut", 5);
                        }
			        }
		        }
            }
            
		}
	}
	
	
}
