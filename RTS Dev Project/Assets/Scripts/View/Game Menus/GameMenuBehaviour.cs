using UnityEngine;
using System.Collections;

public class GameMenuBehaviour : MonoBehaviour {

	public string mainMenuSceneName = "Menu";

	[SerializeField] private GameObject escGameMenu;
	[SerializeField] private GameObject endGameMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!endGameMenu.activeSelf && !escGameMenu.activeSelf && Input.GetKey(KeyCode.Escape)){
			escGameMenu.SetActive(true);
		}
			
	}

	public void ToMainMenu(){
		Application.LoadLevel ("Menu");
	}

	public void EndGameMenu(bool victory){
		ToMainMenu ();
		//endGameMenu.GetComponent<EndGameMenu> ().endGame (victory);//.victory = victory;
		//endGameMenu.SetActive (true);
	}
}
