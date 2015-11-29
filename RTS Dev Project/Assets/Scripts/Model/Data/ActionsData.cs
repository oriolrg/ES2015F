using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class ActionsData : ScriptableObject 
{
    public Dictionary<UnitType, List<UnitType>> creationPermissions = new Dictionary<UnitType, List<UnitType>>()
    {
        { UnitType.Civilian, new List<UnitType>() {UnitType.TownCenter, UnitType.Barracs, UnitType.Stable, UnitType.Archery, UnitType.Wonder } },
        { UnitType.Soldier, new List<UnitType>() { } },
        { UnitType.Knight, new List<UnitType>() { } },
        { UnitType.Archer, new List<UnitType>() { } },
        { UnitType.Carpenter, new List<UnitType>() { UnitType.Farm, UnitType.Carpentry, UnitType.Minery, UnitType.Academy, UnitType.Windmill } },
        { UnitType.Farmer, new List<UnitType>() { UnitType.Farm } },

        { UnitType.TownCenter, new List<UnitType>() {UnitType.Civilian, UnitType.Carpenter, UnitType.Farmer } },
        { UnitType.Barracs, new List<UnitType>() { UnitType.Soldier } },
        { UnitType.Stable, new List<UnitType>() { UnitType.Knight } },
        { UnitType.Archery, new List<UnitType>() {UnitType.Archer } },
        { UnitType.Minery, new List<UnitType>() { } },
        { UnitType.Farm, new List<UnitType>() { UnitType.Farmer } },
        { UnitType.Carpentry, new List<UnitType>() { UnitType.Carpenter } },
        { UnitType.Academy, new List<UnitType>() { } },
        { UnitType.Windmill, new List<UnitType>() { } },
        { UnitType.Tower, new List<UnitType>() { UnitType.Soldier, UnitType.Knight } },

        { UnitType.Objective, new List<UnitType>() { } },
        {UnitType.Wonder, new List<UnitType>() { } }
    };
    public List<MoveData> movements;
    public List<SpecialData> specials;
}
