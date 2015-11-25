using UnityEngine;
using UnityEngine.UI;
using System;


[Serializable]
public class StatSpriteDictionary : SerializableDictionary<Stat, Sprite> { }

[Serializable]
public class CivilizationDataDictionary : SerializableDictionary<Civilization, CivilizationData> { }

[Serializable]
public class StatValueDictionary : SerializableDictionary<Stat, float> { }

[Serializable]
public class ResourceTextDictionary : SerializableDictionary<Resource, Text> { }

[Serializable]
public class ResourceValueDictionary : SerializableDictionary<Resource, int> { }

[Serializable]
public class ActionGroupPanelDictionary : SerializableDictionary<ActionGroup, GameObject> { }

[Serializable]
public class UnitDataDictionary : SerializableDictionary<UnitType, UnitData> { }

[Serializable]
public class UnitPrefabDictionary : SerializableDictionary<UnitType, GameObject> { }

[Serializable]
public class CivilizationValueDictionary : SerializableDictionary<Civilization, float> { }

[Serializable]
public class PlayersDataDictionary : SerializableDictionary<Player, PlayerData> { }