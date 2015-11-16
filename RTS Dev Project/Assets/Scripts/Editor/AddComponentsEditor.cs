using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AddComponentsEditor : MonoBehaviour
{

    public static List<Type> unitsTypes = new List<Type>
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
        foreach (Type type in unitsTypes)
        {
            if (selected.GetComponent(type) == null)
            {
                selected.AddComponent(type);
            }
        }
        BoxCollider collider = selected.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    public static List<Type> buildingTypes = new List<Type>()
    {
        typeof(BoxCollider),
        typeof(Animator),
        typeof(updateGrid),
        typeof(BuildingConstruction),
        typeof(Identity),
        typeof(DelayedActionQueue),
        typeof(Spawner),
        typeof(Health),
        typeof(LOSEntity)
    };


    [MenuItem("Add Components/Add Building Components")]
    public static void AddComponentsBuilding()
    {
        GameObject selected = Selection.activeGameObject;
        foreach (Type type in buildingTypes)
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
