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

	public void ToMainMenu(string scene){
		Application.LoadLevel (scene);
	}

	public void EndGameMenu(Vector3 poi, bool victory, string reason){
		print("EndGameMenu method in GameMenuBehaviour");

		Vector3 newCameraPosition;
		
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (
			ray, out hit,
			Mathf.Infinity // max distance
			)
	    ) {
			newCameraPosition = poi + (Camera.main.transform.position - hit.point);
		} else {
			throw new UnityException("lastBuilding isn't over ground!");
		}

		Camera.main.transform.position = newCameraPosition;

		Time.timeScale = 0; // pause the game
		ToMainMenu("EndGameScene"); // open the EndGameScene in 3 seconds
		//endGameMenu.GetComponent<EndGameMenu> ().endGame (victory, reason);
	}
}
