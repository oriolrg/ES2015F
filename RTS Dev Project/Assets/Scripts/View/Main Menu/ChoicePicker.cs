using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoicePicker : MonoBehaviour {

	private Text label;

	// State variables
	[SerializeField] private string variableName;

	[SerializeField] private List<string> options = new List<string>();
	[SerializeField] private string defaultOption;

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
			throw new UnityException ("Default Option not in options");
		else {
			ChangeState (defaultOptionIndex);
		}
	}
	
	public string GetCurrentOption(){
		return this.options [this.currentOption];
	}
	
	public List<string> getOptions(){
		return options;
	}

	public void SetOptions(List<string> options){
		if (options.Count == 0)
			throw new UnityException("Trying to set options in ChoicePicker wit no options");

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
