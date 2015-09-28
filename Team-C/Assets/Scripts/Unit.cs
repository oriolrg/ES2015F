using UnityEngine;
using System.Collections.Generic;


public class Unit : Selectable 
{
    void Start()
    {
        actions = new List<Action>() { DestroyUnit, Upgrade };
        stats = new Dictionary<string, float>()
        {
            { "health" , 100 },
            { "attack" , 5 },
            { "deffense", 2 }

        };
    }

    void DestroyUnit()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }


}
