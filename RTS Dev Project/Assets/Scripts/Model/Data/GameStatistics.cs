using UnityEngine;
using System;
using System.Collections;

public class GameStatistics : MonoBehaviour {
	
	public class PlayerStatistics {
		public int CreatedUnits;
		public int LostUnits;
		public int KilledUnits;
		public int WoodCollected;
		public int StoneCollected;
		public int FoodCollected;
		
		public PlayerStatistics(){
			CreatedUnits = 0;
			LostUnits = 0;
			KilledUnits = 0;
			WoodCollected = 0;
			StoneCollected = 0;
			FoodCollected = 0;
		}
	}

	private static PlayerStatistics player = new PlayerStatistics();
	private static PlayerStatistics cpu1 = new PlayerStatistics();
	private static PlayerStatistics cpu2 = new PlayerStatistics();
	private static PlayerStatistics cpu3 = new PlayerStatistics();
	public static Player winner = Player.Player;
	public static Victory winCondition = Victory.Annihilation;

	public static void resetStatistics(){
		player = new PlayerStatistics();
		cpu1 = new PlayerStatistics();
		cpu2 = new PlayerStatistics();
		cpu3 = new PlayerStatistics();
	}

	public static PlayerStatistics getPS(Player p){
		if (p.Equals(Player.Player))
			return player;
		else if (p.Equals (Player.CPU1))
			return cpu1;
		else if (p.Equals (Player.CPU2))
			return cpu2;
		else if (p.Equals (Player.CPU3))
			return cpu3;
		else
			throw new UnityException(String.Format("Player {0} doesn't have statistics", p));
	}

	public static void addCreatedUnits(Player p, int x){ getPS(p).CreatedUnits += x; }
	public static void addLostUnits(Player p, int x){ getPS(p).LostUnits += x; }
	public static void addKilledUnits(Player p, int x){ getPS(p).KilledUnits += x; }
	public static void addWoodCollected(Player p, int x){ getPS(p).WoodCollected += x; }
	public static void addStoneCollected(Player p, int x){ getPS(p).StoneCollected += x; }
	public static void addFoodCollected(Player p, int x){ getPS(p).FoodCollected += x; }
}
