using UnityEngine;
using System.Collections;

public class CollectResources : MonoBehaviour {

	public int totalFood;
	public Transform targetToCollect;
	public bool hasCollected;
	UnitMovement u;
	
	// Use this for initialization
	void Start () {
		u = GetComponent<UnitMovement>();
		hasCollected = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startMovingToStorage(Transform t){
		hasCollected = true;
		u.startMovingCollect(t);
	}

	public void startMovingToCollect(Transform t){
		hasCollected = false;
		u.startMovingCollect(t);
	}
}
