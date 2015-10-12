using UnityEngine;
using UnityEditor;
using System.IO;

public class DataMenuEditor : MonoBehaviour 
{

	[MenuItem("Data/Create Unit Data")]
    public static void CreateUnitPack()
    {
        UnitData data = ScriptableObject.CreateInstance<UnitData>();
        
        AssetDatabase.CreateAsset(data, "Assets/Data/NewUnitData"+Random.Range(0,1000)+".asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

    [MenuItem("Data/Create UI Settings")]
    public static void CreateUISettings()
    {
        UISettings data = ScriptableObject.CreateInstance<UISettings>();

        AssetDatabase.CreateAsset(data, "Assets/Data/UISettings.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

    [MenuItem("Data/Create Civilization Data")]
    public static void CreateCivilizationData()
    {
        CivilizationData data = ScriptableObject.CreateInstance<CivilizationData>();

        AssetDatabase.CreateAsset(data, "Assets/Data/Civilizations/newCivilization"+Random.Range(0,1000)+".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

    [MenuItem("Data/Save Data")]
    public static void SaveData()
    {
        AssetDatabase.Refresh();
        string[] assetFiles = Directory.GetFiles(Application.dataPath, "*.asset", SearchOption.AllDirectories);
        foreach (string assetFile in assetFiles)
        {
            string assetPath = "Assets" + assetFile.Replace(Application.dataPath, "").Replace('\\', '/');
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            EditorUtility.SetDirty(asset);
            print(assetPath + " saved ");
        }
        AssetDatabase.SaveAssets();
    }

    public static void aaa() { }


}
