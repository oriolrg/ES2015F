using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class ActionsData : ScriptableObject 
{
    public Dictionary<UnitType, List<UnitType>> creationPermissions = new Dictionary<UnitType, List<UnitType>>()
    {
        { UnitType.Civilian, new List<UnitType>() {UnitType.TownCenter, UnitType.Barracs, UnitType.Stable, UnitType.Archery } },
        { UnitType.Soldier, new List<UnitType>() { } },
        { UnitType.Knight, new List<UnitType>() { } },
        { UnitType.Archer, new List<UnitType>() { } },

        { UnitType.TownCenter, new List<UnitType>() {UnitType.Civilian } },
        { UnitType.Barracs, new List<UnitType>() { UnitType.Soldier } },
        { UnitType.Stable, new List<UnitType>() { UnitType.Knight } },
        { UnitType.Archery, new List<UnitType>() {UnitType.Archer } }
    };
    public List<MoveData> movements;
    public List<SpecialData> specials;
}
