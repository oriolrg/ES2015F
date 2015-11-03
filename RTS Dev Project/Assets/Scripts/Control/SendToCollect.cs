using UnityEngine;
using System.Collections;

public class SendToCollect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position,10);
		foreach(Collider c in hitColliders){
            if (c.tag == "Enemy"||c.tag=="Ally")
            {
                CollectResources collect = c.GetComponent<CollectResources>();
                if (collect == null) { }
                else if (collect.hasCollected & c.tag == gameObject.tag) storeResource(c.gameObject);
                else if (!collect.goingToCollect & c.tag == "Enemy" & gameObject.tag == "Enemy") AI.Instance.reassignResourceToCivilian(c.gameObject);

            }
			
		}
	}

    private void storeResource(GameObject c)
    {
        CollectResources collect = c.GetComponent<CollectResources>();
        if (gameObject.tag == "Ally") GameController.Instance.updateResource(collect.resourceCollected, -collect.quantityCollected);
        else if(gameObject.tag == "Enemy") AI.Instance.updateResource(collect.resourceCollected, -collect.quantityCollected);
        collect.quantityCollected = 0;
        collect.hasCollected = false;
        if (collect.targetToCollect != null) collect.startMovingToCollect(collect.targetToCollect);
    }
}


