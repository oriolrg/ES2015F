using UnityEngine;

using System;
using System.Collections.Generic;



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
