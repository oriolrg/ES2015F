using UnityEngine;
using System.Collections.Generic;
public class Building : Selectable 
{
	
    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
        stats = new Dictionary<string, float>()
        {
            { "health" , 100 },
            { "deffense", 2 }

        };
    }

    void CreateUnit()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    void DestroyBuilding()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
