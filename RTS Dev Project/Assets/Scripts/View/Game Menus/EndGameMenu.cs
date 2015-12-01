using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameMenu : MonoBehaviour {

	public string victoryText = "You won!";
	public string defeatText = "You lost :(";

	public bool victory = true;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		Time.timeScale = 0;
	}

	public void endGame(bool victory, string reason){
		print(string.Format("EndGame {0} {1}", victory.ToString (), reason));
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

		//print(gameObject.activeSelf);
		gameObject.SetActive(true);
		//print(gameObject.activeSelf);
	}

	void OnDisable() { Time.timeScale = 1; }
}
