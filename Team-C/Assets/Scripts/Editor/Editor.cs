using UnityEngine;
using UnityEditor;

public class Editor : MonoBehaviour 
{
	[MenuItem("Data/Create Unit Data")]
    public static void CreateUnitPack()
    {
        Data data = ScriptableObject.CreateInstance<Data>();
        
        AssetDatabase.CreateAsset(data, "Assets/Data/NewUnitData.asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }


}
