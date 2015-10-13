using UnityEngine;

using System;
using System.Collections.Generic;

[Serializable]
public class UnitData : ScriptableObject 
{
    public Sprite preview;
    public List<Sprite> actionSprites;
    public List<Action> actions;
    public StatValueDictionary stats;
}
