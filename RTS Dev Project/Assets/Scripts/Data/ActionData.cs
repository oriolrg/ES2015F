using UnityEngine;

public class ActionData : ScriptableObject
{
    public ActionGroup actionGroup;
    public string description;
    public Sprite sprite;
    public ResourceValueDictionary resourceCost;
    public float requiredTime;
}