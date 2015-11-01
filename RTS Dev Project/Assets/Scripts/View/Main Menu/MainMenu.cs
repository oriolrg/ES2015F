using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] private GameObject loadingScreen;

	[SerializeField] private ChoicePicker gameDiff, map, playerCivilization;
	[SerializeField] private List<GameObject> cpuBoxes;
	[SerializeField] private List<TextToggle> winConditions;
	[SerializeField] private List<GameObject> maps;

	void Start() {
//		List<string> mapNames = new List<string>(maps.Count);
//		foreach (GameObject m in maps)
//			mapNames.Add(m.GetComponent<MapData>().name);
//		
//		map.GetComponent<ChoicePicker>().SetOptions(mapNames);
	}

	public void NewGame(string scene){
		GameData.diff = GetEnumValue<GameData.DifficultyEnum>(gameDiff.GetCurrentOption ());

		// Set map
//		bool mapFound = false;
//		foreach (GameObject m in maps){
//			if (m.GetComponent<MapData>().name == map.GetCurrentOption()){
//				GameData.map = m;
//				mapFound = true;
//				break;
//			}
//		}
//
//		if (!mapFound)
//			throw new UnityException("Picked map not found in maps list!");

		// Set winConditions
		GameData.winConditions.Clear ();
		foreach (TextToggle t in winConditions) {
			if (t.isActive()){
				GameData.winConditions.Add (GetEnumValue<GameData.WinConditionEnum>(t.getValue()));
			}
		}

		// Set civilization of all players and skill of CPUs
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

		// Check if all GameConditions are correct
		if (GameData.GameConditionsCorrect()){
			// Everything's fine. Start game
			StartCoroutine( LoadScene(scene) );
		}
	}
	
	IEnumerator LoadScene(string scene)
	{
		// Coroutine that opens the loading screen for 3 seconds and loads the game
		if (loadingScreen != null){
			loadingScreen.SetActive(true);
			yield return new WaitForSeconds (3.0f);
		}
		
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
