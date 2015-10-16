using UnityEngine;
using System;
using System.Collections.Generic;

public class Civil : Unit
{

    [SerializeField] private GameObject buildingPrefab;

    [SerializeField]
    private float dist;

    private GameObject buildingToConstruct;
    // private bool construct;

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateBuilding, DestroyUnit };
    }



    void Start()
    {

        dist = 2f;
        construct = false;
    }

    void Update()
    {
        //If a unit has the order to construct and it is close enough to the building, start the construction
        if (construct)
        {
            Debug.Log("holaaa");
            if ((transform.position - buildingToConstruct.transform.position).magnitude < dist)
            {
                Debug.Log("A CONSTRUIIIIR!!!!!!!!");
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction(this.gameObject);
                construct = false;
                GetComponent<Unit>().SetInConstruction(true);
            }
        }
    }


    public void DestroyUnit()
    {
            if (construct)
            {
                construct = false;
            }

            GameController.Instance.removeUnit(gameObject);
            GetComponentInParent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        
    }


    public void CreateBuilding()
    {
        
            if (construct)
            {
                construct = false;
            }

            GameController.Instance.createBuilding(buildingPrefab);
        
    }


    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }
}