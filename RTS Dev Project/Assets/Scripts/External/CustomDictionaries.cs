using UnityEngine.UI;
using System;

// Concrete types of generic classes to be able to serialize them within Unity.
[Serializable]
public class StatValueDictionary : SerializableDictionary<StatType, float> { }

[Serializable]
public class ResourceTextDictionary : SerializableDictionary<Resource, Text> { }