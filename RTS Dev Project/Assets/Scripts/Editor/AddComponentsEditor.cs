using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AddComponentsEditor : MonoBehaviour
{

    public static List<Type> types = new List<Type>
    {
        typeof(BoxCollider),
        typeof(Seeker),
        typeof(Identity),
        typeof(LOSEntity),
        typeof(Health),
        typeof(DelayedActionQueue),
        typeof(CharacterController),
        typeof(UnitMovement)
    };

    [MenuItem("Add Components/Add Unit Componenets")]
    public static void AddComponentsUnit()
    {
        GameObject selected = Selection.activeGameObject;
        foreach (Type type in types)
        {
            if (selected.GetComponent(type) == null)
            {
                selected.AddComponent(type);
            }
        }
        BoxCollider collider = selected.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

}
