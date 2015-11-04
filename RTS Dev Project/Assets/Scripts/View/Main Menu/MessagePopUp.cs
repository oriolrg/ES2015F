using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessagePopUp : MonoBehaviour {

	[SerializeField] private Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowMessage(string message){
		text.text = message;
		gameObject.SetActive(true);
	}
}
