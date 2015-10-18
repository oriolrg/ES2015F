using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoicePicker : MonoBehaviour {

	private Text label;

	// State variables
	[SerializeField] private string variableName;
	[SerializeField] private List<string> options;
	[SerializeField] private string defaultOption;
	private int defaultOptionIndex;

	private int currentOption;

	[SerializeField] private List<ChoicePickerChangeStateListener> listeners;

	// Use this for initialization
	void Start () {
		// Initialize variables
		label = GetComponent<Text>();

		defaultOptionIndex = 0;
		foreach (string s in options) {
			if (s.Equals (defaultOption))
				break;
			else
				defaultOptionIndex++;
		}

		if (defaultOptionIndex >= options.Count)
			throw new UnityException ("Default Option not in options");
		else {
			ChangeState (defaultOptionIndex);
		}
	}

	public void ChangeState(){
		currentOption++;

		if (currentOption >= options.Count)
			currentOption = 0;

		ChangeState (currentOption);
	}

	private void ChangeState(int op){
		label.text = variableName + ": " + options [op];			
		currentOption = op;

		foreach (ChoicePickerChangeStateListener listener in listeners) {
			listener.OnChangeState(options[currentOption]);
		}
	}
}
