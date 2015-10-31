using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Civilian : MobileUnit
{
    [SerializeField] private GameObject urbanCenterPrefab;
    [SerializeField] private GameObject wonderPrefab;

    [SerializeField]
    private float dist;

    //private GameObject buildingToConstruct;

    public GameObject dustPrefab;
    public GameObject usingDust;

    // private bool construct;

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateUrbanCenter, CreateWonder, Move, Stop, Sacrifice };
    }



    void Start()
    {
        
        //construct = false;

        //dist = 10f;
    }

    void Update()
    {
        
        /*
        //If a unit has the order to construct and it is close enough to the building, start the construction
        if (construct)
        {
            Debug.Log("dins de construct civilian");
            if ((transform.position - buildingToConstruct.transform.position).magnitude < dist)
            {
                Debug.Log("A CONSTRUIIIIR!!!!!!!!");
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction(this.gameObject);
                construct = false;
                GetComponent<Unit>().SetInConstruction(true);
                usingDust = Instantiate(dustPrefab, buildingToConstruct.transform.position, Quaternion.identity) as GameObject;
            }
        }
        if (inConstruction)
            GetComponentInParent<Animator>().SetBool("running", false);
        if(inConstruction == false && usingDust != null)
        {
            Destroy(usingDust);
        }
        */

    }

    public void CreateUrbanCenter()
    {
        /*
        if (construct || inConstruction)
        {
            construct = false;
            inConstruction = false;
            buildingToConstruct.GetComponent<BuildingConstruction>().deleteUnit(this.gameObject);
        }*/

        if (GameController.Instance.checkResources(data.actions[0].resourceCost)) GameController.Instance.createBuilding(urbanCenterPrefab);

    }

    public void CreateWonder()
    {
        /*
        if (construct || inConstruction)
        {
            construct = false;
            inConstruction = false;
            buildingToConstruct.GetComponent<BuildingConstruction>().deleteUnit(this.gameObject);
        }*/

        if(GameController.Instance.checkResources(data.actions[0].resourceCost)) GameController.Instance.createBuilding(wonderPrefab);

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
        /*
            if (construct || inConstruction)
            {
                construct = false;
                inConstruction = false;
                buildingToConstruct.GetComponent<BuildingConstruction>().deleteUnit(this.gameObject);
        }*/

            GameController.Instance.removeUnit(gameObject);
            GetComponentInParent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        
    }



    /*
    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }*/
}