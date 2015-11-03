using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextToggle : MonoBehaviour {

	private Text text;
	private string label;

	[SerializeField] private bool toggleActive = false;
	[SerializeField] private string value = "";

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		label = text.text;

		UpdateText ();
	}

	public void Toggle(){
		toggleActive = !toggleActive;
		UpdateText ();
	}

	private void UpdateText(){
		if (toggleActive)
			text.text = label + " √";
		else
			text.text = label;
	}

	public bool isActive(){
		return toggleActive;
	}

	public string getValue(){
		return value;
	}
}
