using UnityEngine;
using System.Collections.Generic;
using System;

public class Troop 
{
    [SerializeField] public List<GameObject> units;
    public GameObject FocusedUnit;

    public Troop()
    {
        units = new List<GameObject>();
        FocusedUnit = null;
        
    }
    public Troop(List<GameObject> units)
    {
        this.units = units;
        if (units.Count > 0)
            FocusedUnit = units[0];
    }
}