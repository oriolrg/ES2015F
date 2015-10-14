using UnityEngine;
using System.Collections.Generic;
using System;

public class Troop : MonoBehaviour
{
    [SerializeField] public List<Unit> units;
    public Unit FocusedUnit;
}