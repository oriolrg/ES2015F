﻿using UnityEngine;

using System;
using System.Collections.Generic;


[Serializable]
public enum StatType { Health, Attack, Defense, Speed, Range, Cost_Food, Cost_Wood, Cost_Metal, Construction_Time }
// Concrete types of generic classes to be able to serialize them within Unity.
[Serializable]
public class StatValueDictionary : SerializableDictionary<StatType, float> { }

[Serializable]
public class UnitData : ScriptableObject 
{
    public Sprite preview;
    public List<Sprite> actionSprites;
    public StatValueDictionary stats;
}