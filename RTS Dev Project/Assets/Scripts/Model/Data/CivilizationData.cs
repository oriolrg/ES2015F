using UnityEngine;
using System;

[Serializable]
public class CivilizationData : ScriptableObject
{
    public string name;

	public Color color;

    public Material material;

    public Sprite FlagSprite;

    public Sprite PanelSprite;

    public Font font;

    public UnitPrefabDictionary units;

    void Start()
    {
        material.color = color;
    }
}
