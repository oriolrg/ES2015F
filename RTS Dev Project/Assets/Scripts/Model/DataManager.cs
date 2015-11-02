
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public CivilizationDataDictionary civilizationDatas;
    public UnitDataDictionary unitDatas;

    public Dictionary<Civilization, List<string>> names = new Dictionary<Civilization, List<string>>()
    {
        {Civilization.Greeks, new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" }}
    };

    public List<string> adjectives = new List<string>() { "Important", "Lazy", "Popular", "Historical", "Scared", "Old", "Traditional", "Strong", "Helpful", "Competitive", "Legal", "Obvious" };




    public static DataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}