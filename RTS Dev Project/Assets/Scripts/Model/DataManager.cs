
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerData player;
    public PlayerData CPU1;
    public PlayerData CPU2;
    public PlayerData CPU3;
	public PlayerData Neutral;

    public CivilizationDataDictionary civilizationDatas;
    public PlayersDataDictionary playerDatas;
    public UnitDataDictionary unitDatas;

    public Dictionary<Civilization, List<string>> names = new Dictionary<Civilization, List<string>>()
    {
        {Civilization.Greeks, new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" }},
        {Civilization.Egyptians, new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" }},
        {Civilization.Neutral, new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" }},
		{Civilization.Babylonians, new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" }}
    };

    public List<string> adjectives = new List<string>() { "Important", "Lazy", "Popular", "Historical", "Scared", "Old", "Traditional", "Strong", "Helpful", "Competitive", "Legal", "Obvious" };

    public static DataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        playerDatas.Add(Player.Player, player);
        playerDatas.Add(Player.CPU1, CPU1);
        playerDatas.Add(Player.CPU2, CPU2);
        playerDatas.Add(Player.CPU3, CPU3);
		playerDatas.Add (Player.Neutral, Neutral);
    }
}