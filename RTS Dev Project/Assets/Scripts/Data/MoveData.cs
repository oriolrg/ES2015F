using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class MoveData : ScriptableObject 
{
    public string description;
    public Sprite preview;
    public Event codeToExecute;
}
