using UnityEngine;
using System.Collections.Generic;
using System;

public class Troop 
{
    [SerializeField] public List<GameObject> units;
    public GameObject FocusedUnit; // should be an index instead of a gameobject.!!!

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

    public void focusNext()
    {
        if (FocusedUnit == null) return;

        int index = units.IndexOf(FocusedUnit);
        if (index == units.Count - 1)
            FocusedUnit = units[0];
        else
            FocusedUnit = units[index + 1];
    }

    public bool hasMovableUnits()
    {
        foreach( GameObject unit in units )
        {
            Identity identity = unit.GetComponentOrEnd<Identity>();

            if (!identity.unit.isBuilding())
                return true;
        }
        return false;
    }
}