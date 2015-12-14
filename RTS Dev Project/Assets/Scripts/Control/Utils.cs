using UnityEngine;
using System;
using System.Collections;


public class Utils : MonoBehaviour {
	public static T GetEnumValue<T> (string name) { 
		/**
		 * Returns enum value from enum T with name 'name'
		 */
		return (T) Enum.Parse (typeof(T), name);
	}

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}
