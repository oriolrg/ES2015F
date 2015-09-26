using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class DetectNearbyUnits : MonoBehaviour {

	public int nearbyAlliedUnits;
	public int nearbyEnemyUnits;
	public Text countText;

	// Use this for initialization
	void Start () {

		nearbyAlliedUnits = 1;
		nearbyEnemyUnits = 0; 
		countText.text = "Allied units: " + nearbyAlliedUnits 
			+ "\nEnemy units: " + nearbyEnemyUnits;
	
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Player_Unit") {
			nearbyEnemyUnits += 1;
		}

		if(other.gameObject.tag == "CPU_Unit") {
			nearbyAlliedUnits += 1;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player_Unit") {
			nearbyEnemyUnits -= 1;
		} else if (other.gameObject.tag == "CPU_Unit") {
			nearbyAlliedUnits -= 1;
		}
	}
}
