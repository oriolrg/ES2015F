using UnityEngine;
using UnityEditor;
using System.IO;

public class DataMenuEditor : MonoBehaviour 
{
    public static void CreateAsset<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(data, string.Format("Assets/Data/{0}/{1}.asset",path, Random.Range(0, 1000)));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

	[MenuItem("Data/Create Unit Data")]
    public static void CreateUnitData()
    {
        CreateAsset<UnitData>("Units");
    }

    [MenuItem("Data/Create HUD Data")]
    public static void CreateUISettings()
    {
        CreateAsset<HUDData>("UI");
    }

    [MenuItem("Data/Create Civilization Data")]
    public static void CreateCivilizationData()
    {
        CreateAsset<CivilizationData>("Civilizations");
    }

    [MenuItem("Data/Create Action Data")]
    public static void CreateActionData()
    {
        CreateAsset<ActionData>("Actions");
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
}
