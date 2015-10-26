using UnityEngine;
using System;

[Serializable]
public class CivilizationData : ScriptableObject
{
	public string name;

	public Color color;

    public Sprite FlagSprite;

    public Sprite PanelSprite;

    public Font font;
}