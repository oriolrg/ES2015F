using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataEditorPicker : MonoBehaviour {

	public GameData.DifficultyEnum diff = GameData.DifficultyEnum.Medium;
	public Victory winCondition = Victory.MapControl;

	public Civilization playerCiv;

	public GameData.DifficultyEnum cpu1Skill;
	public Civilization cpu1Civ;

	public GameData.DifficultyEnum cpu2Skill;
	public Civilization cpu2Civ;

	public GameData.DifficultyEnum cpu3Skill;
	public Civilization cpu3Civ;

	public GameObject map;
	public bool sceneFromMenu = true;

	// Use this for initialization
	public void AddFakeGameData ()
    {
		GameData.diff = diff;
		GameData.winConditions.Add (winCondition);

		GameData.player = new GameData.PlayerData(playerCiv);

		if (cpu1Skill != GameData.DifficultyEnum.None && cpu1Civ != Civilization.Neutral)
			GameData.cpus.Add (new GameData.CPUData(cpu1Civ, cpu1Skill));
		if (cpu2Skill != GameData.DifficultyEnum.None && cpu2Civ != Civilization.Neutral)
			GameData.cpus.Add (new GameData.CPUData(cpu2Civ, cpu2Skill));
		if (cpu3Skill != GameData.DifficultyEnum.None && cpu3Civ != Civilization.Neutral)
			GameData.cpus.Add (new GameData.CPUData(cpu3Civ, cpu3Skill));

		GameData.map = map;
		GameData.sceneFromMenu = sceneFromMenu;
	}

	void Awake() {}
	void Start() {}
}
