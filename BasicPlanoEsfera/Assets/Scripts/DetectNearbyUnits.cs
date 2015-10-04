using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class DetectNearbyUnits : MonoBehaviour {

	public int nearbyAlliedUnits;
	public int nearbyEnemyUnits;
	public Text countText;
	public float maxDistance;
	public float radius;

	// Use this for initialization
	void Start () {

		nearbyAlliedUnits = 1;
		nearbyEnemyUnits = 0; 
		maxDistance = 5;
		radius = 2;
		countText.text = "Allied units: " + nearbyAlliedUnits 
			+ "\nEnemy units: " + nearbyEnemyUnits;
	
	}
	
	// Update is called once per frame
	void Update () {

		nearbyAlliedUnits = 1;
		nearbyEnemyUnits = 0;

		Collider[] hitColliders = Physics.OverlapSphere(transform.position,radius);
		foreach(Collider c in hitColliders){
			if(c.tag == "ally_Unit"){
				nearbyAlliedUnits += 1;
			}
			if(c.tag == "enemy_Unit"){
				nearbyEnemyUnits += 1;
			}
		}


		countText.text = "Allied units: " + nearbyAlliedUnits 
			+ "\nEnemy units: " + nearbyEnemyUnits;
		if (nearbyEnemyUnits == 0) {
			countText.text = countText.text + "\nAction: No enemies on sight. Keep moving.";
		}
		if (nearbyEnemyUnits > nearbyAlliedUnits) {
			countText.text = countText.text + "\nAction: More enemies than allies. ¡Run!";
		}
		if (nearbyEnemyUnits <= nearbyAlliedUnits && nearbyEnemyUnits > 0) {
			countText.text = countText.text + "\nAction: More allies than enemies. ¡Kill Them!";
		}
	}
}
