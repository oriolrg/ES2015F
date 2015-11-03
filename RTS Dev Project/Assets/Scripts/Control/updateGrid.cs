using UnityEngine;
using System.Collections;
using Pathfinding;

public class updateGrid : MonoBehaviour {

	// Use this for initialization
	public bool isBuildingPlaced = false;
	
	void Update(){
		if(isBuildingPlaced){
			Bounds bou;
			BoxCollider b = GetComponent<BoxCollider>();
			if(b != null){
				bou = b.bounds;
				var guo = new GraphUpdateObject(bou); 
				guo.updatePhysics = true;
				AstarPath.active.UpdateGraphs(guo);
				isBuildingPlaced = false;
			}
		}
	}
}
