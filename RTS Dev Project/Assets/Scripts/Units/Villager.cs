using UnityEngine;
using System.Collections.Generic;

public class Villager : Focusable
{
    [SerializeField] private GameObject buildingPrefab;
	[SerializeField] private AnimationClip deathAnimation;

    [SerializeField] private float dist;

    private GameObject buildingToConstruct;
   // private bool construct;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
        //GameController.Instance.addAllyUnit(gameObject);

        dist = 2f;
        construct = false;
    }

    void Update()
    {
        //If a unit has the order to construct and it is close enough to the building, start the construction
        if(construct)
        {
            Debug.Log("holaaa");
            if((transform.position - buildingToConstruct.transform.position).magnitude < dist)
            {
                Debug.Log("A CONSTRUIIIIR!!!!!!!!");
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction(this.gameObject);
                construct = false;
                GetComponent<Focusable>().SetInConstruction(true);
            }
        }
    }


    public void DestroyUnit()
    {
        
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {

            if (construct)
            {
                construct = false;
            }

            GameController.Instance.removeAllyUnit(gameObject);
            GetComponentInParent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
            GameController.Instance.ClearSelection();
        }
    }


    public void CreateBuilding()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            if (construct)
            {
                construct = false;
            }

            GameController.Instance.createBuilding(buildingPrefab);
        }
    }


    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }


    //public void ActivateBuildingToConstruct()
    //{
      //  construct = true;
    //}


}