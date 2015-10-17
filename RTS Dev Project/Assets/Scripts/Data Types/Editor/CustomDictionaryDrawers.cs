using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(StatValueDictionary))]
public class StatValueDictionaryDrawer : DictionaryDrawer<Stat, float> { }

[CustomPropertyDrawer(typeof(StatSpriteDictionary))]
public class StatSpriteDictionaryDrawer : DictionaryDrawer<Stat, Sprite> { }

[CustomPropertyDrawer(typeof(CivilizationDataDictionary))]
public class CivilizationDataDictionaryDrawer : DictionaryDrawer<Civilization, CivilizationData> { }

[CustomPropertyDrawer(typeof(ResourceTextDictionary))]
public class ResourceTextDictionaryDrawer : DictionaryDrawer<Resource, Text> { }

[CustomPropertyDrawer(typeof(ResourceValueDictionary))]
public class ResourceValueDictionaryDrawer : DictionaryDrawer<Resource, int> { }

[CustomPropertyDrawer(typeof(ActionGroupPanelDictionary))]
public class ActionGroupPanelDictionaryDrawer : DictionaryDrawer<ActionGroup, GameObject> { }
