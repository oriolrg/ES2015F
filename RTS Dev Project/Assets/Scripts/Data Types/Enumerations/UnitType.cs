using System;

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

    public static string toString(this UnitType unit)
    {
        return unit.toString();
    }
}