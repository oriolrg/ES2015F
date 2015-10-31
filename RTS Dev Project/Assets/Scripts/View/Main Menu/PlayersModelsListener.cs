using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersModelsListener : ChoicePickerChangeStateListener {

	[SerializeField] private List<GameObject> civilizations;
	//[SerializeField] private GameObject romans;

	public override void OnChangeState(string state){
		foreach (GameObject go in civilizations){
			go.SetActive (go.name.Equals(state));
		}
	}

	public override void OnChangeActive(bool active){
		gameObject.SetActive(active);
	}
}
