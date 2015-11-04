using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoicePicker : MonoBehaviour {

	private Text label = null;

	// State variables
	[SerializeField] private string variableName = "choicePicker";

	[SerializeField] private List<string> options = new List<string>();
	[SerializeField] private string defaultOption = null;

	public int currentOption { get; private set; }

	[SerializeField] private List<ChoicePickerChangeStateListener> listeners;

	// Use this for initialization
	void Start () {
		int defaultOptionIndex = 0;

		// Initialize variables
		label = GetComponent<Text>();

		foreach (string s in options) {
			if (s.Equals (defaultOption))
				break;
			else
				defaultOptionIndex++;
		}

		if (defaultOptionIndex >= options.Count)
			defaultOptionIndex = 0;

		ChangeState (defaultOptionIndex);
	}
	
	public string GetCurrentOption(){
		return this.options [this.currentOption];
	}
	
	public List<string> getOptions(){
		return options;
	}

	public void SetOptions(List<string> options){
		if (options.Count == 0)
			throw new UnityException("Trying to set options in ChoicePicker with no options");

		this.options = options;
		ChangeState(0);
	}

	public void ChangeState(){
		currentOption++;

		if (currentOption >= options.Count)
			currentOption = 0;

		ChangeState (currentOption);
	}

	private void ChangeState(int op){
		if (options == null || variableName == null || label == null)
			return;

		label.text = variableName + ": " + options [op];			
		currentOption = op;

		foreach (ChoicePickerChangeStateListener listener in listeners) {
			listener.OnChangeState(options[currentOption]);
		}
	}

	void OnEnable(){
		foreach (ChoicePickerChangeStateListener listener in listeners) {
			listener.OnChangeActive(gameObject.activeInHierarchy);
		}
	}

	void OnDisable(){
		foreach (ChoicePickerChangeStateListener listener in listeners) {
			listener.OnChangeActive(gameObject.activeInHierarchy);
		}
	}
}
