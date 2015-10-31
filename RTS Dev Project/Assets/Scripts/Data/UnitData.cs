using UnityEngine;

using System;
using System.Collections.Generic;

[Serializable]
public class UnitData : ScriptableObject 
{
    public string description;
    public Sprite preview;
    public ResourceValueDictionary resourceCost;
    public StatValueDictionary stats;

    public float requiredTime;
}
