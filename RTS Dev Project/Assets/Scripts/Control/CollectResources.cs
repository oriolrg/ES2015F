using UnityEngine;
using System.Collections;

public class CollectResources : MonoBehaviour {
    
	public GameObject targetToCollect;
	public bool hasCollected;
	public bool goingToCollect;
	public UnitMovement u;
    public Resource resourceCollected;
    public int quantityCollected;
	
	// Use this for initialization
    void Awake()
    {
        quantityCollected = 0;
        u = GetComponent<UnitMovement>();
		hasCollected = false;
		goingToCollect = false;
    }

	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<BoxCollider>().bounds.extents.x * 3);
		foreach(Collider c in hitColliders){
			if (System.Enum.IsDefined(typeof(Resource), c.tag))
			{}
			Identity id = c.gameObject.GetComponent<Identity>();
			if(id!=null){
				if(id.unitType==UnitType.TownCenter){
					print (name);
					if (hasCollected & c.tag == gameObject.tag){
						storeResource();
					}
					else if (!goingToCollect & tag == "Enemy" & c.tag == "Enemy") AI.Instance.reassignResourceToCivilian(gameObject);
				}
			}
			
		}

	}

	public void startMovingToStorage()
    {
        GameObject t = AI.Instance.getClosestTownCenter(gameObject);
        hasCollected = true;
        goingToCollect = false;
		u.startMoving(t);
	}

	public void startMovingToCollect(){
        hasCollected = false;        
        goingToCollect = true;
        u.startMoving(targetToCollect);
	}



	private void storeResource()
	{
		if (gameObject.tag == "Ally") GameController.Instance.updateResource(resourceCollected, -quantityCollected);
		else if(gameObject.tag == "Enemy") AI.Instance.updateResource(resourceCollected, -quantityCollected);
		quantityCollected = 0;
		hasCollected = false;
		if (targetToCollect != null) startMovingToCollect();
	}
}
