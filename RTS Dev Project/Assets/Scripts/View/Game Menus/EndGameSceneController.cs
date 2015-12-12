using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EndGameSceneController : MonoBehaviour {

	public string MainMenuScene = "Menu";

	private Text victoryText;
	private List<GameObject> playerStatistics;

	// Use this for initialization
	void Start () {
		victoryText = gameObject.transform.GetChild (0).GetComponent<Text>();
		string victoryString = String.Format ("{0} won,\nby {1}", GameStatistics.winner, GameStatistics.winCondition);

		victoryText.text = victoryString;

		// Get PlayerStatistics GameObjects
		GameObject parent = (GameObject) gameObject.transform.GetChild (1).gameObject;
		playerStatistics = new List<GameObject>();

		// Add them to the list
		for (int i=0; i<4; i++){
			playerStatistics.Add ((GameObject) parent.transform.GetChild (i).gameObject);
			fillPlayer(playerStatistics[i], i);
		}

		// Active only the first ones (those active)
		for (int i=0; i<(GameData.cpus.Count+1); i++)
			playerStatistics[i].SetActive(true);

		for (int i=(GameData.cpus.Count+1); i<4; i++)
			playerStatistics[i].SetActive(false);
	}

	public void ToMainMenu(){
		Application.LoadLevel(MainMenuScene);
	}
	
	// Update is called once per frame
	void Update () {}

	private void fillPlayer(GameObject g, int i){
		Player p;
		switch (i){
			case 0: p = Player.Player; break;
			case 1: p = Player.CPU1; break;
			case 2: p = Player.CPU2; break;
			case 3: p = Player.CPU3; break;
			default: throw new UnityException(String.Format("Player {0} doesn't have statistics", i));
		}

		GameStatistics.PlayerStatistics ps = GameStatistics.getPS (p);

		setText(g, 0, p.ToString()); // player name
		setText(g, 1, String.Format (
			"Created units: {0}", ps.CreatedUnits.ToString()
		)); // created units
        setText(g, 2, String.Format (
			"Lost units: {0}", ps.LostUnits.ToString()
		)); // lost units
		setText(g, 3, String.Format (
			"Killed units: {0}", ps.KilledUnits.ToString()
		)); // killed units
		setText(g, 4, String.Format (
			"Wood collected: {0}", ps.WoodCollected.ToString()
		)); // wood collected
		setText(g, 5, String.Format (
			"Stone collected: {0}", ps.StoneCollected.ToString()
		)); // stone collected
		setText(g, 6, String.Format (
			"Food collected: {0}", ps.FoodCollected.ToString()
		)); // food collected
	}

	private void setText(GameObject g, int i, string text){
		g.transform.GetChild (i).GetComponent<Text>().text = text;
	}
}
