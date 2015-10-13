using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    public List<DelayedAction> DelayedActions{ get; private set; }
    public float HealthRatio { get { return health * 1f / maxHealth; }}

    [SerializeField] private UnitData data;
    public Sprite Preview { get { return data.preview; } }
    
}
