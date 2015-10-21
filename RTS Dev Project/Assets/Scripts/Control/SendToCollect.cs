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
            if (c.tag == "Enemy"||c.tag=="Ally")
            {
                CollectResources collect = c.GetComponentInParent<CollectResources>();
                if (collect == null)
                {
                    // Doesn't have component, so doesn't collect
                    return;
                }
                else if (!collect.goingToCollect)
                {
                    if (collect.targetToCollect != null)
                    {
                        collect.startMovingToCollect(collect.targetToCollect);
                    }
                    else if (c.tag == "Enemy")
                    {
                        AI.Instance.reassignResourceToCivilian(c.gameObject);
                    }
                }
                

            }
			
		}
	}
}
