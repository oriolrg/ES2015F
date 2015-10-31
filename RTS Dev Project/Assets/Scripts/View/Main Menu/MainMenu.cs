using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] private ChoicePicker gameDiff, map, playerCivilization;
	[SerializeField] private List<GameObject> cpuBoxes;
	[SerializeField] private List<TextToggle> winConditions;
	[SerializeField] private List<GameObject> maps;

	void Start() {
	}

	public void NewGame(string scene){
		GameData.diff = GetEnumValue<GameData.DifficultyEnum>(gameDiff.GetCurrentOption ());
		//GameData.map = GetEnumValue<GameData.DifficultyEnum>(gameDiff.GetCurrentOption ());

		GameData.winConditions.Clear ();
		foreach (TextToggle t in winConditions) {
			if (t.isActive()){
				GameData.winConditions.Add (GetEnumValue<GameData.WinConditionEnum>(t.getValue()));
			}
		}

		String civ, skill;

		GameData.player = new GameData.PlayerData (
			GetEnumValue<GameData.PlayerData.CivilizationEnum>(
				playerCivilization.GetCurrentOption()
			)
		);

		GameData.cpus.Clear ();
		foreach (GameObject cpu in cpuBoxes){
			if (cpu.activeSelf){
				skill = cpu.transform.Find("CPUDIff").GetComponent<ChoicePicker>().GetCurrentOption();
				civ = cpu.transform.Find("CPUCiv").GetComponent<ChoicePicker>().GetCurrentOption();

				GameData.cpus.Add (
					new GameData.CPUData(
						GetEnumValue<GameData.PlayerData.CivilizationEnum> (civ),
						GetEnumValue<GameData.DifficultyEnum> (skill)
					)
				);
			}
		}

		// Tell GameController that it should initialize game with GameData
		GameData.sceneFromMenu = true; 

		/*try {
			if (GameData.GameConditionsCorrect())
				Application.LoadLevel (scene);
		} catch (GameData.GameConditionsException ex) {

		}*/

		if (GameData.GameConditionsCorrect())
			Application.LoadLevel (scene);
	}

	public void Quit(){
		print ("Quit");
		Application.Quit ();
	}
	

	public static T GetEnumValue<T> (string name) { 
		return (T) Enum.Parse (typeof(T), name);
	}
}
