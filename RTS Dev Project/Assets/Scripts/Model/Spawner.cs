
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector3 SpawningPoint { get; private set; }
    [HideInInspector] public Vector3 RallyPoint { get; set; }

    void Start()
    {
        initBounds();
    }

    public void initBounds()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (box != null)
        {
            SpawningPoint = transform.position + transform.forward * (box.bounds.extents.z + 1);
        }
        else
        {
            SpawningPoint = transform.position + transform.forward * 3;
        }
        RallyPoint = SpawningPoint + transform.forward * 5;
    }
}