using UnityEngine;
using System.Collections.Generic;

public class Troop : MonoBehaviour
{
    [SerializeField] public List<Unit> units;
    public Unit FocusedUnit { get; set; }
}