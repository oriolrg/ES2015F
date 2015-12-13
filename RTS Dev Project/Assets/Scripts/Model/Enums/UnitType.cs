using System;
using UnityEngine;

[Serializable]
public enum UnitType
{
    Civilian,
    Soldier,
    Knight,
    Archer,
    Farmer,
    Carpenter,

    TownCenter,
    Barracs,
    Stable,
    Archery,
    Minery,
    Windmill,
    Carpentry,
    Farm,
    Academy,
    Tower,

    Objective,
    Wonder,
    Wall,
    Door
}

public static class UnitExtensions
{
    public static bool isBuilding(this UnitType unit)
    {
        return
            unit == UnitType.TownCenter ||
            unit == UnitType.Barracs ||
            unit == UnitType.Stable ||
            unit == UnitType.Archery ||
            unit == UnitType.Objective ||
            unit == UnitType.Wonder ||
            unit == UnitType.Minery ||
            unit == UnitType.Windmill ||
            unit == UnitType.Carpentry ||
            unit == UnitType.Farm ||
            unit == UnitType.Academy ||
            unit == UnitType.Tower ||
            unit == UnitType.Door ||
            unit == UnitType.Wall;
    }
    public static T GetComponentOrEnd<T>(this GameObject go)
    {
        T result = go.GetComponent<T>();

        if (result == null)
        {
            Debug.LogError("Required component not found in " + go.name + ". Aborting");
            //EditorApplication.isPlaying = false;
            Application.Quit();
        }

        return result;
    }
}