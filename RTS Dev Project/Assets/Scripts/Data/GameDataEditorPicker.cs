using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataEditorPicker : MonoBehaviour {

	public GameData.DifficultyEnum diff = GameData.DifficultyEnum.Medium;
	public GameData.WinConditionEnum winCondition = GameData.WinConditionEnum.Domination;

	public GameData.PlayerData.CivilizationEnum playerCiv;

	public GameData.DifficultyEnum cpu1Skill;
	public GameData.PlayerData.CivilizationEnum cpu1Civ;

	public GameData.DifficultyEnum cpu2Skill;
	public GameData.PlayerData.CivilizationEnum cpu2Civ;

	public GameData.DifficultyEnum cpu3Skill;
	public GameData.PlayerData.CivilizationEnum cpu3Civ;

	public GameObject map;
	public bool sceneFromMenu = true;

	// Use this for initialization
	public void AddFakeGameData () {
		GameData.diff = diff;
		GameData.winConditions.Add (winCondition);

		GameData.player = new GameData.PlayerData(playerCiv);

		if (cpu1Skill != null && cpu1Civ != null)
			GameData.cpus.Add (new GameData.CPUData(cpu1Civ, cpu1Skill));
		if (cpu2Skill != null && cpu2Civ != null)
			GameData.cpus.Add (new GameData.CPUData(cpu2Civ, cpu2Skill));
		if (cpu3Skill != null && cpu3Civ != null)
			GameData.cpus.Add (new GameData.CPUData(cpu3Civ, cpu3Skill));

		GameData.map = map;
		GameData.sceneFromMenu = sceneFromMenu;
	}

	void Awake() {}
	void Start() {}
}
