using UnityEngine;
using System;
using System.Collections.Generic;

public class Civilian : Unit
{

    [SerializeField] private GameObject wonderPrefab;

    [SerializeField]
    private float dist;

    private GameObject buildingToConstruct;
    // private bool construct;

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateWonder, Move, Stop, Sacrifice };
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

    public void CreateWonder()
    {

        if (construct)
        {
            construct = false;
        }

        GameController.Instance.createBuilding(wonderPrefab);

    }

    public void Move()
    {

        // move to a specific place
        print("not implemented yet");
    }


    public void Stop()
    {
        // move to a specific place
        print("not implemented yet");
    }



    public void Sacrifice()
    {
            if (construct)
            {
                construct = false;
            }

            GameController.Instance.removeUnit(gameObject);
            GetComponentInParent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        
    }




    


    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }
}