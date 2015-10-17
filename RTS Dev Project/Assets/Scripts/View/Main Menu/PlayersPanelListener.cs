using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersPanelListener : ChoicePickerChangeStateListener {

	[SerializeField] private GameObject cpuBox1;
	[SerializeField] private GameObject cpuBox2;
	[SerializeField] private GameObject cpuBox3;

	// Use this for initialization
	void Start () {
	}


	public override void OnChangeState(string state){
		switch (state) {
		case "2":
			cpuBox2.SetActive(false);
			cpuBox3.SetActive(false);
			break;
		case "3":
			cpuBox2.SetActive(true);
			cpuBox3.SetActive(false);
			break;
		case "4":
			cpuBox2.SetActive(true);
			cpuBox3.SetActive(true);
			break;
		}
	}
}
