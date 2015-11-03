using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainListener : ChoicePickerChangeStateListener {

	[SerializeField] private GameObject mountains;
	[SerializeField] private GameObject desert;
	
	// Use this for initialization
	void Start () {
	}
	
	
	public override void OnChangeState(string state) {
		switch (state) {
		case "Mountains":
			mountains.SetActive (true);
			desert.SetActive (false);
			break;
		case "Desert":
			mountains.SetActive (false);
			desert.SetActive (true);
			break;
		}
	}
	
	public override void OnChangeActive(bool active) {
		// Need to override, but don't do anything
	}
}
