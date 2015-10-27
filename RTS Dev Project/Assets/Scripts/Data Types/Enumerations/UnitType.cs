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

public class UnitUtils
{
    public static bool isBuilding(UnitType unit)
    {
        return 
            unit == UnitType.TownCenter || 
            unit == UnitType.Barracs || 
            unit == UnitType.Stable || 
            unit == UnitType.Archery;
    }
}