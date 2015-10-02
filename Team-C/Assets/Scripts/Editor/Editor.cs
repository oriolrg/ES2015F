using UnityEngine;
using UnityEditor;

public class Editor : MonoBehaviour 
{
	[MenuItem("Data/Create Unit Data")]
    public static void CreateUnitPack()
    {
        UnitData data = ScriptableObject.CreateInstance<UnitData>();
        
        AssetDatabase.CreateAsset(data, "Assets/Data/NewUnitData.asset");
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


}
