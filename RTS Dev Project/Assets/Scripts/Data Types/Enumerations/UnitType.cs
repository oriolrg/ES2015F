﻿using System;
using UnityEngine;
using UnityEditor;
[Serializable]
public enum UnitType
{
    Civilian,
    Soldier,
    Knight,
    Archer,

    TownCenter,
    Barracs,
    Stable,
    Archery
    
}

public static class UnitExtensions
{
    public static bool isBuilding(this UnitType unit)
    {
        return 
            unit == UnitType.TownCenter || 
            unit == UnitType.Barracs || 
            unit == UnitType.Stable || 
            unit == UnitType.Archery;
    }

    public static T GetComponentOrEnd<T>( this GameObject go )
    {
        T result = go.GetComponent<T>();
        
        if( result == null )
        {
            Debug.LogError("Required component not found in " + go.name +". Aborting");
            EditorApplication.isPlaying = false;
            Application.Quit();
        }

        return result;
    }
}