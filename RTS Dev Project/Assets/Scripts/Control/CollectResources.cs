using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void VoidMethod();

public class CollectResources : MonoBehaviour
{
	public UnitMovement unitMovement;
    public ResourceValueDictionary resourceBank;
    
    public GameObject targetObject;
    public Animator animator;
	public bool goingToCollect;

    public static Dictionary<Resource, string> gatheringAnimationBools = new Dictionary<Resource, string>()
    {
        {Resource.Food, "cultivate" },
        {Resource.Wood, "cut" },
        {Resource.Metal, "chop" }
    };

    void Awake()
    {
		goingToCollect = false;
        unitMovement = GetComponent<UnitMovement>();
        animator = GetComponent<Animator>();
        resourceBank = new ResourceValueDictionary();
        foreach( Resource resource in Enum.GetValues(typeof(Resource)))
        {
            resourceBank.Add(resource, 0);
        }
    }

    public void startMovingToCollect(GameObject targetResource)
    {
		goingToCollect = true;
        targetObject = targetResource;
        unitMovement.startMoving( targetObject, collect);
    }

    private void addResource()
    {
        DestroyOnExpend destroyOnExpend = targetObject.GetComponent<DestroyOnExpend>();

        if (destroyOnExpend != null)
        {
            destroyOnExpend.amount -= 2;

            Resource resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), targetObject.tag);

            resourceBank[resourceCollected] += 2;

			GameController.Instance.hud.updateRightPanel(gameObject);


        }
        else
        {
            Debug.LogError("No destroyOnExpend script found in target object");
        }
    }

    

    public void collect()
    {
        print("collecting");

        InvokeRepeating("addResource", 1, 1);


        // Animate during 5 seconds

        if( animator != null )
        {
            Resource resourceCollected = (Resource)System.Enum.Parse(typeof(Resource), targetObject.tag);

            animator.SetBool(gatheringAnimationBools[resourceCollected], true);
        }

        Invoke("startMovingToStorage", 5.1f);
        
    }

    public void startMovingToStorage()
    {
		goingToCollect = false;
        CancelInvoke("addResource");

        GameObject targetTownCenter = AI.Instance.getClosestTownCenter(gameObject);

		unitMovement.startMoving( targetTownCenter, store );
	}

    public void store()
    {
        if( tag == "Ally" )
        {
            foreach( KeyValuePair<Resource, int> kv in resourceBank )
            {
                GameController.Instance.updateResource(kv.Key, -kv.Value);
            }
            
        }
        else if( tag == "Enemy" )
        {
            foreach (KeyValuePair<Resource, int> kv in resourceBank)
            {
                AI.Instance.updateResource(kv.Key, -kv.Value);
            }
        }

        foreach( Resource key in resourceBank.Keys)
        {
            resourceBank[key] = 0;
        }

        if (animator != null)
        {
            animator.SetBool("cultivate", true);
        }

        Invoke("returnToCollect", 1);

    }

    public void returnToCollect()
    {
		GameController.Instance.hud.updateRightPanel(gameObject);
        startMovingToCollect(targetObject);
    }

    public int totalCollected()
    {
        int res = 0;
        foreach(int value in resourceBank.Values)
        {
            res += value;
        }

        return res;
    }
}
