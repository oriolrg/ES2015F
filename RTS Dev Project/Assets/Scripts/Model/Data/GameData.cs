using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public static readonly short maxCPUPlayers = 3;

	public enum DifficultyEnum { Easy, Medium, Hard, None };
	public static DifficultyEnum diff = DifficultyEnum.Medium;

	public static List<Victory> winConditions = new List<Victory>(3);

	public static PlayerData player;
	public static List<CPUData> cpus = new List<CPUData>(maxCPUPlayers);

	public static GameObject map;

	// Bool that tells GameController if it should use this data to initialize the game or not
	public static bool sceneFromMenu = false;

	public static bool GameConditionsCorrect() {
		if (winConditions.Count < 1)
			throw new GameConditionsException ("No win condition selected!");
		else if (cpus.Count < 1)
			throw new GameConditionsException ("No CPU selected!");
		else
			return true;
	}

	public class PlayerData {
		public enum CivilizationEnum { Greeks, Egyptians, Babylonians, None };
		public CivilizationEnum civ { get; private set; }

		public PlayerData(CivilizationEnum civ){
			this.civ = civ;
		}
	}

	public class CPUData : PlayerData {
		public DifficultyEnum skill { get; private set; }

		public CPUData(CivilizationEnum civ, DifficultyEnum skill) : base(civ){
			this.skill = skill;
		}
	}

	public class GameConditionsException: UnityEngine.UnityException {
		public GameConditionsException(string message) : base(message) {}
	}
}
