using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Edit if you want the menu to pass info to the game
	[SerializeField] private bool useMenuInfo = true;

	[SerializeField] private GameObject loadingScreen;
	[SerializeField] private MessagePopUp messagePopUp;

	[SerializeField] private ChoicePicker gameDiff, map, playerCivilization;
	[SerializeField] private List<GameObject> cpuBoxes;
	[SerializeField] private List<TextToggle> winConditions;
	[SerializeField] private List<GameObject> maps;

	void Start() {
		List<string> mapNames = new List<string>(maps.Count);
		foreach (GameObject m in maps)
			mapNames.Add(m.GetComponent<MapInfo>().mapName);
		
		map.GetComponent<ChoicePicker>().SetOptions(mapNames);
	}

	public void NewGame(string scene){
		GameData.diff = Utils.GetEnumValue<GameData.DifficultyEnum>(gameDiff.GetCurrentOption ());

		// Set map
		bool mapFound = false;
		foreach (GameObject m in maps){
			if (m.GetComponent<MapInfo>().mapName.Equals(map.GetCurrentOption())){
				GameData.map = m;
				mapFound = true;
				break;
			}
		}

		if (!mapFound)
			throw new UnityException("Picked map not found in maps list!");

		// Set winConditions
		GameData.winConditions.Clear ();
		foreach (TextToggle t in winConditions) {
			if (t.isActive()){
				GameData.winConditions.Add (Utils.GetEnumValue<Victory>(t.getValue()));
			}
		}

		// Set civilization of all players and skill of CPUs
		String civ, skill;

		GameData.player = new GameData.PlayerData (
			Utils.GetEnumValue<Civilization>(
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
						Utils.GetEnumValue<Civilization> (civ),
						Utils.GetEnumValue<GameData.DifficultyEnum> (skill)
					)
				);
			}
		}

		// Tell GameController that it should initialize game with GameData
		GameData.sceneFromMenu = useMenuInfo; 

		// Check if all GameConditions are correct
		try{
			if (GameData.GameConditionsCorrect()){
				// Everything's fine. Start game
				StartCoroutine( LoadScene(scene) );
			}
		} catch (GameData.GameConditionsException e){
			messagePopUp.ShowMessage(e.Message);
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
}
