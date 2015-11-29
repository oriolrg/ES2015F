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
	/*
	void OnEnable(){
		print ("HELLOOOOOOO");
		string message;


		if (victory)
			message = victoryText;

		else
			message = defeatText;
		print (message);
		foreach (Transform t in transform) {
			if (t.gameObject.name.Equals ("Text")){
				t.gameObject.GetComponent<Text>().text = message;
				break;
			}
		}

		

		Time.timeScale = 0;
	}
*/
	public void endGame(bool victory, string reason){
		gameObject.SetActive (true);
		this.victory = victory;
		string message;		
		if (victory)
			message = victoryText;
		
		else
			message = defeatText;
		foreach (Transform t in transform) {
			if (t.gameObject.name.Equals ("Text")){
				t.gameObject.GetComponent<Text>().text = message + '\n' + reason;
				break;
			}
		}
		Time.timeScale = 0;

	}
	void OnDisable() { Time.timeScale = 1; }
}
