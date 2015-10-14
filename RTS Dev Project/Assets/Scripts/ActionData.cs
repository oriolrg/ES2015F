using UnityEngine;

public class ActionData : ScriptableObject
{
    public string description;
    public Sprite sprite;
    public ResourceValueDictionary resourceCost;
    public float requiredTime;
    public bool needsExtraInput;
}