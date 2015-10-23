using UnityEngine;
using System.Collections;

public class CollectResources : MonoBehaviour {
    
	public GameObject targetToCollect;
	public bool hasCollected;
	public bool goingToCollect;
	public UnitMovement u;
	
	// Use this for initialization
	void Start () {
		u = GetComponent<UnitMovement>();
		hasCollected = false;
		goingToCollect = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startMovingToStorage(GameObject t){
		hasCollected = true;
        goingToCollect = false;
		u.startMoving(t);
	}

	public void startMovingToCollect(GameObject t){
		hasCollected = false;
        goingToCollect = true;
        u.startMoving(t);
	}
}
