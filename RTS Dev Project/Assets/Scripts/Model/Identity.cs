
using System.Collections.Generic;
using UnityEngine;

public class Identity : MonoBehaviour
{
    [SerializeField]
    public Civilization civilization;

    [SerializeField]
    public Player player;

    [SerializeField]
    public UnitType unitType;
    

    void Start()
    {

        // Create random name
        List<string> adjectives = DataManager.Instance.adjectives;
		if(unitType.Equals(UnitType.Wonder)){
			if(gameObject.tag=="Ally") 
				GameController.Instance.winCondition();
			else if(gameObject.tag=="Enemy") 
				GameController.Instance.loseCondition();
		}
        if (unitType.isBuilding())
            name = string.Format("The {0} {1}", adjectives[Random.Range(0, adjectives.Count)], unitType.ToString() );
        else
        {
            List<string> names = DataManager.Instance.names[civilization];
            
            name = string.Format("{0}, The {1}", names[Random.Range(0, names.Count)], adjectives[Random.Range(0, adjectives.Count)]);
        }
        if (unitType == UnitType.TownCenter) AI.Instance.addTownCenter(gameObject);
        if (unitType == UnitType.Civilian)	AI.Instance.assignCivilian (gameObject);
		BuildingConstruction build = GetComponent<BuildingConstruction> ();
		if (build != null) {
			if (!build.getConstructionOnGoing()){
				GameController.Instance.addUnit (gameObject);
			}
		}
        else
        {
            GameController.Instance.addUnit(gameObject);
        }

    }
}
