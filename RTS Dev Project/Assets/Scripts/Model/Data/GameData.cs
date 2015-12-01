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

	public static Player cpuIdToPlayer(int id){
		switch(id) {
		case 0: return Player.CPU1;
		case 1: return Player.CPU2;
		case 2: return Player.CPU3;
		default: return Player.Neutral;
		}
	}

	public static int playerToCPUId(Player p){
		if (p.Equals (Player.CPU1))
		    return 0;
	    else if (p.Equals (Player.CPU2))
		    return 1;
	    else if (p.Equals (Player.CPU3))
		    return 2;
		else
			throw new UnityException("Player not CPU");
	}
	
	public static Civilization playerToCiv(Player p) {
		if (p.Equals(Player.Player))
			return player.civ;
		else if (p.Equals(Player.Neutral))
			return Civilization.Neutral;
		else
			return cpus[playerToCPUId(p)].civ;
	}

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
		public Civilization civ { get; private set; }

		public PlayerData(Civilization civ){
			this.civ = civ;
		}
	}

	public class CPUData : PlayerData {
		public DifficultyEnum skill { get; private set; }

		public CPUData(Civilization civ, DifficultyEnum skill) : base(civ){
			this.skill = skill;
		}
	}

	public class GameConditionsException: UnityEngine.UnityException {
		public GameConditionsException(string message) : base(message) {}
	}
}
