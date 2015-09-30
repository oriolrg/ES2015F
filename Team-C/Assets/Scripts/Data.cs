using UnityEngine;
using System;

[Serializable]
public class StatDictionary : SerializableDictionary<string, float> { }

[Serializable]
public class Data : ScriptableObject 
{
    public StatDictionary stats;
    public float UnitHealth     = 100;
    public float UnitAttack     = 100;
    public float UnitDefense    = 100;
    public float UnitMovement   = 100;
    public float UnitMaximum    = 100;
}
