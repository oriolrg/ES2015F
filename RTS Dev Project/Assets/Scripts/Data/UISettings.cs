﻿using UnityEngine;

using System;
using System.Collections.Generic;

// Concrete types of generic classes to be able to serialize them within Unity.
[Serializable]
public class StatSpriteDictionary : SerializableDictionary<StatType, Sprite> { }
[Serializable]
public class CivilizationDataDictionary : SerializableDictionary<Civilization, CivilizationData> { }

[Serializable]
public class UISettings : ScriptableObject
{
    public CivilizationDataDictionary civilizationDatas;
    public StatSpriteDictionary statSprites;
}
