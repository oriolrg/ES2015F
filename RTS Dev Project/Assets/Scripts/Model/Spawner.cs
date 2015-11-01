
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector3 spawningPoint;
    [HideInInspector]
    public Vector3 rallyPoint;

    void Start()
    {
        rallyPoint = spawningPoint;
    }
}