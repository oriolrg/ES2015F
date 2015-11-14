using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameMenu : MonoBehaviour {

	public string victoryText = "You won!";
	public string defeatText = "You lost :(";

	public bool victory = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		string message;

		if (victory)
			message = victoryText;
		else
			message = defeatText;

		foreach (Transform t in transform) {
			if (t.gameObject.name.Equals ("Text")){
				t.gameObject.GetComponent<Text>().text = message;
				break;
			}
		}

		gameObject.SetActive(true);

		Time.timeScale = 0;
	}

	void OnDisable() { Time.timeScale = 1; }
}
