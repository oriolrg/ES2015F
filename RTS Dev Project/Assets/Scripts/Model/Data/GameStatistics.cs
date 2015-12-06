using UnityEngine;
using System.Collections;

public class GameStatistics : MonoBehaviour {
	
	class PlayerStatistics {
		public int CreatedUnits;
		public int KilledUnits;
		public int DeadUnits;
		public int WoodCollected;
		public int StoneCollected;
		public int FoodCollected;
		
		public PlayerStatistics(){
			CreatedUnits = 0;
			KilledUnits = 0;
			DeadUnits = 0;
			WoodCollected = 0;
			StoneCollected = 0;
			FoodCollected = 0;
		}
	}

	private static PlayerStatistics player = new PlayerStatistics();
	private static PlayerStatistics cpu1 = new PlayerStatistics();
	private static PlayerStatistics cpu2 = new PlayerStatistics();
	private static PlayerStatistics cpu3 = new PlayerStatistics();

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
			raise UnityException(string.format("Player {0} doesn't have statistics", p.ToString()));
	}

	public static int addCreatedUnits(Player p, int x){ getPS(p).CreatedUnits += x; }
	public static int addKilledUnits(Player p, int x){ getPS(p).KilledUnits += x; }
	public static int addDeadUnits(Player p, int x){ getPS(p).DeadUnits += x; }
	public static int addWoodCollected(Player p, int x){ getPS(p).WoodCollected += x; }
	public static int addStoneCollected(Player p, int x){ getPS(p).StoneCollected += x; }
	public static int addFoodCollected(Player p, int x){ getPS(p).FoodCollected += x; }
}
