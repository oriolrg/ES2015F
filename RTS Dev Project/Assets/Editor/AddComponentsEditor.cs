using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AddComponentsEditor : MonoBehaviour 
{

	public static List<Type> types = new List<Type>()
	{
		typeof(BoxCollider),
		typeof(EXPLORAR),
		typeof(updateGrid),
		typeof(BuildingConstruction),
		typeof(Identity),
        typeof(DelayedActionQueue),
        typeof(Spawner)
	};


	[MenuItem("Add Components/Add Building Components")]
	public static void AddComponentsBuilding()
	{
		GameObject selected = Selection.activeGameObject;
		foreach( Type type in types)
		{
			if(selected.GetComponent(type) == null)
			{
				selected.AddComponent(type);
			}
		}

		BoxCollider collider = selected.GetComponent<BoxCollider>();
		collider.isTrigger = true;
	}
}
