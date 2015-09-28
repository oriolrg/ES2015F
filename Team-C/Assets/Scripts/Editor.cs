using UnityEngine;
using UnityEditor;

public class Editor : MonoBehaviour 
{
	[MenuItem("Data/Create Settings")]
    public static void CreateSettings()
    {
        Data data = ScriptableObject.CreateInstance<Data>();
        
        AssetDatabase.CreateAsset(data, "Assets/Configuration/Settings.asset");
        AssetDatabase.SaveAssets();
        

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = data;
    }


}
