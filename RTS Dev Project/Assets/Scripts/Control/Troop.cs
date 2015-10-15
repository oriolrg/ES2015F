using UnityEngine;
using System.Collections.Generic;
using System;

public class Troop : MonoBehaviour
{
    [SerializeField] public List<GameObject> units;
    public Unit FocusedUnit;

    void Start()
    {
        units = new List<GameObject>();
        FocusedUnit = null;
    }
}