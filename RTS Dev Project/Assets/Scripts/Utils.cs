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
}
