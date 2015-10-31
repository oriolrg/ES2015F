using UnityEngine;
using System.Collections;
using Pathfinding;

public class updateGrid : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		var guo = new GraphUpdateObject(GetComponent<Collider>().bounds); 
		guo.updatePhysics = true;
		AstarPath.active.UpdateGraphs(guo);

	}
}
