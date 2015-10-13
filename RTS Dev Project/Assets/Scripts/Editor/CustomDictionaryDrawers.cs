using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(StatValueDictionary))]
public class StatValueDictionaryDrawer : DictionaryDrawer<StatType, float> { }

[CustomPropertyDrawer(typeof(StatSpriteDictionary))]
public class StatSpriteDictionaryDrawer : DictionaryDrawer<StatType, Sprite> { }

[CustomPropertyDrawer(typeof(CivilizationDataDictionary))]
public class CivilizationDataDictionaryDrawer : DictionaryDrawer<Civilization, CivilizationData> { }

[CustomPropertyDrawer(typeof(ResourceTextDictionary))]
public class ResourceTextDictionaryDrawer : DictionaryDrawer<Resource, Text> { }

[CustomPropertyDrawer(typeof(ResourceValueDictionary))]
public class ResourceValueDictionaryDrawer : DictionaryDrawer<Resource, int> { }
