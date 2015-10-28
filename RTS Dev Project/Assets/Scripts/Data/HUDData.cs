using UnityEngine;

using System;

[Serializable]
public class HUDData : ScriptableObject
{
    public CivilizationDataDictionary civilizationDatas;
    public GameObject blockPrefab;
    public GameObject overlappedTimeFrame;
    public Sprite focusSprite;
}
