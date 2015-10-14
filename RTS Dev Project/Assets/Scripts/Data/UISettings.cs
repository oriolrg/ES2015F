using UnityEngine;

using System;

[Serializable]
public class UISettings : ScriptableObject
{
    public CivilizationDataDictionary civilizationDatas;
    public GameObject blockPrefab;
    public GameObject overlappedTimeFrame;
}
