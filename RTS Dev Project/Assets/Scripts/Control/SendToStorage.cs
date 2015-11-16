using UnityEngine;
using System.Collections.Generic;

public class SendToStorage : MonoBehaviour
{
    public static Dictionary<Resource, string> gatheringAnimationBools = new Dictionary<Resource, string>()
    {
        {Resource.Food, "cultivate" },
        {Resource.Wood, "cut" },
        {Resource.Metal, "chop" }
    };
    
    Resource myResource;
    GameObject collector;

    void Start()
    {
        if (tag != "Untagged")
            myResource = (Resource)System.Enum.Parse(typeof(Resource), this.tag);
    }

    public void stopAnimations()
    {
        // Stop the gathering animation
        collector.GetComponent<Animator>().SetBool(gatheringAnimationBools[myResource], false);
    }

    void OnTriggerEnter(Collider c)
    {
        // Get the unit we collide with
        GameObject unit = c.gameObject;

        print("collecting");

        // Check that the unit is going to collect this resource
        CollectResources collect = unit.GetComponentInParent<CollectResources>();
        DestroyOnExpend d = GetComponent<DestroyOnExpend>();

        if( collect != null && d != null && collect.goingToCollect && collect.targetToCollect == gameObject )
        {
            // Set this unit as collector
            collector = unit;

            // Collect 10 resources
            d.amount -= 10;
            collect.resourceCollected = myResource;
            collect.quantityCollected += 10;

            // Animate during 5 seconds
            Animator animator = unit.GetComponent<Animator>();

            animator.SetBool(gatheringAnimationBools[myResource], true);

            // Call stop animations and start moving to storage after 5 seconds
            Invoke("stopAnimations", 5);
            collect.Invoke("startMovingToStorage", 5);
        }
    }

    void OnTriggerExit(Collider c)
    {
        // Get the unit we collide with
        GameObject unit = c.gameObject;
        unit.GetComponent<Animator>().SetBool(gatheringAnimationBools[myResource], false);
        unit.GetComponent<CollectResources>().CancelInvoke("startMovingToStorage");
    }
	
	
}
