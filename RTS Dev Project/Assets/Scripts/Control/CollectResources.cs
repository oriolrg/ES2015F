using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void VoidMethod();

public class CollectResources : MonoBehaviour
{
	public UnitMovement unitMovement;
    public Resource resourceCollected;
    public int quantityCollected;
    public GameObject targetObject;

    public static Dictionary<Resource, string> gatheringAnimationBools = new Dictionary<Resource, string>()
    {
        {Resource.Food, "cultivate" },
        {Resource.Wood, "cut" },
        {Resource.Metal, "chop" }
    };

    void Awake()
    {
        quantityCollected = 0;
        unitMovement = GetComponent<UnitMovement>();
    }

    public void startMovingToCollect(GameObject targetResource)
    {
        targetObject = targetResource;
        print("voy");
        unitMovement.startMoving( targetObject, collect);
    }

    

    public void collect()
    {
        print("collecting");

        DestroyOnExpend destroyOnExpend = targetObject.GetComponent<DestroyOnExpend>();

        if( destroyOnExpend != null )
        {
            destroyOnExpend.amount -= 10;

            resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), targetObject.tag);

            quantityCollected += 10;

            // Animate during 5 seconds
            Animator animator = GetComponent<Animator>();

            if( animator != null )
            {
                animator.SetBool(gatheringAnimationBools[resourceCollected], true);
                // Call stop animations and start moving to storage after 5 seconds
                
            }

            Invoke("startMovingToStorage", 5);

        }
        else
        {
            Debug.LogError("No destroyOnExpend script found in target object");
        }


        
    }

    public void startMovingToStorage()
    {
        GameObject targetTownCenter = AI.Instance.getClosestTownCenter(gameObject);

		unitMovement.startMoving( targetTownCenter, store );
	}

    public void store()
    {
        print("he arrivat");

        if( tag == "Ally" )
        {
            GameController.Instance.updateResource(resourceCollected, - quantityCollected);
        }
        else if( tag == "Enemy" )
        {
            AI.Instance.updateResource(resourceCollected, - quantityCollected);
        }

        quantityCollected = 0;

        startMovingToCollect(targetObject);
    }
}
