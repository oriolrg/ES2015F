
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /*
    public Vector3 SpawningPoint { get; private set; }
    [HideInInspector] public Vector3 RallyPoint { get; set; }
    */
    public Vector3 SpawningPoint;
    public Vector3 RallyPoint;
    public bool customRally = false;

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
        if( !customRally )
            RallyPoint = SpawningPoint + transform.forward * 3;
    }
}