
using System.Collections.Generic;
using UnityEngine;

public class Identity : MonoBehaviour
{
    [SerializeField]
    public Civilization civilization;

    [SerializeField]
    public UnitType unit;
    

    void Start()
    {

        // Create random name
        List<string> adjectives = DataManager.Instance.adjectives;

        if (unit.isBuilding())
            name = string.Format("The {0} {1}", adjectives[Random.Range(0, adjectives.Count)], unit.toString() );
        else
        {
            List<string> names = DataManager.Instance.names[civilization];
            
            name = string.Format("{0}, The {1}", names[Random.Range(0, names.Count)], adjectives[Random.Range(0, adjectives.Count)]);
        }
            
    }
}
