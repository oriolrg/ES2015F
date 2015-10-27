using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class ActionsData : ScriptableObject 
{
    public Event creationCodeToExecute;
    public Dictionary<UnitType, List<UnitType>> creationPermissions;
    public List<MoveData> movements;
    public List<SpecialData> specials;
}
