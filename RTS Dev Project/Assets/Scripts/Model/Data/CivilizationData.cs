﻿using UnityEngine;
using System;

[Serializable]
public class CivilizationData : ScriptableObject
{
    public Color color;

    public string name;

    public Sprite FlagSprite;

    public Sprite PanelSprite;

    public Font font;

    public UnitPrefabDictionary units;
}
