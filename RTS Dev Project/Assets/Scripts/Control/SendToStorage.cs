using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SendToStorage : MonoBehaviour
{
    public static Dictionary<Resource, string> gatheringAnimationBools = new Dictionary<Resource, string>()
    {
        {Resource.Food, "cultivate" },
        {Resource.Wood, "cut" },
        {Resource.Metal, "chop" }
    };
    
    Resource myResource;

    List<GameObject> collectors = new List<GameObject>();

    void Start()
    {
        if (tag != "Untagged")
            myResource = (Resource)System.Enum.Parse(typeof(Resource), this.tag);
        InvokeRepeating("check", 1, 2);
    }

    void check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<BoxCollider>().bounds.extents.x * 3);

        foreach ( Collider c in colliders)
        {
            if (c.isTrigger && !collectors.Contains(c.gameObject) )
            {
                process(c);
            }
        }

        collectors = colliders.Select(c => c.gameObject).ToList();
    }

    void process(Collider c)
    {
        // Get the unit we collide with
        GameObject unit = c.gameObject;

        // Check that the unit is going to collect this resource
        CollectResources collect = unit.GetComponentInParent<CollectResources>();
        DestroyOnExpend d = GetComponent<DestroyOnExpend>();

        if( collect != null && d != null && collect.goingToCollect && collect.targetToCollect == gameObject )
        {
            // Add to collectors to not recheck
            collectors.Add(unit);

            // Stop the unit
            //unit.GetComponent<UnitMovement>().enabled = false;

            // Collect 10 resources
            d.amount -= 10;
            collect.resourceCollected = myResource;
            collect.quantityCollected += 10;

            // Animate during 5 seconds
            Animator animator = unit.GetComponent<Animator>();

            animator.SetBool(gatheringAnimationBools[myResource], true);
            // Call stop animations and start moving to storage after 5 seconds
            collect.Invoke("startMovingToStorage", 5);
        }
    }
	
}
