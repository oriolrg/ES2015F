
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector3 SpawningPoint { get; private set; }
    [HideInInspector] public Vector3 RallyPoint { get; set; }

    void Start()
    {
        BoxCollider script = GetComponent<BoxCollider>();

        if( script != null )
        {
            SpawningPoint = transform.position + transform.forward * (script.bounds.extents.z + 1);
        }
        else
        {
            SpawningPoint = Vector3.zero;
        }
        RallyPoint = SpawningPoint + transform.forward * 5;
    }
}