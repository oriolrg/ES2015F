using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections.Generic;


// Concrete types of generic classes to be able to serialize them within Unity.
[Serializable]
public class StatDictionary : SerializableDictionary<string, float> { }


[Serializable]
public class Data : ScriptableObject 
{
    public Sprite preview;
    public List<Sprite> actionSprites;
    public StatDictionary stats;
}
