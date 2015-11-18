using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainListener : ChoicePickerChangeStateListener {

	[SerializeField] private GameObject mountains;
	[SerializeField] private GameObject desert;
	[SerializeField] private GameObject meadow;
	
	// Use this for initialization
	void Start () {
	}
	
	
	public override void OnChangeState(string state) {
		switch (state) {
		case "Mountain Map":
			mountains.SetActive (true);
			desert.SetActive (false);
			meadow.SetActive (false);
			break;
		case "Desert Map":
			mountains.SetActive (false);
			desert.SetActive (true);
			meadow.SetActive (false);
			break;
		case "Meadow Map":
			meadow.SetActive (true);
			desert.SetActive (false);
			mountains.SetActive (false);
			break;
		}
	}
	
	public override void OnChangeActive(bool active) {
		// Need to override, but don't do anything
	}
}
