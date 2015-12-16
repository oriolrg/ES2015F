using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {

	private AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = Camera.main.GetComponent<AudioSource>();

		Invoke("ToMenu", 2 * 60 + 6);
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyDown(KeyCode.KeypadEnter)){
		if (Input.GetKeyDown(KeyCode.Return) || 
		    Input.GetKeyDown(KeyCode.KeypadEnter) || 
			Input.GetKeyDown(KeyCode.Escape) || 
			Input.GetKeyDown(KeyCode.Space)
        ){
			ToMenu();
		}
	}

	public void ToMenu() {
		audio.enabled = false;
		Application.LoadLevel("Menu");
	}
}
