﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BuildingConstruction : MonoBehaviour {

    //The different meshes of the building during the construction
    public GameObject initialMesh;
    public GameObject progressMesh;
    private Mesh finalMesh;

    //private bool inConstruction;
    public float timer; //Timer that changes the mesh
    private int phase; //Phase of the construction
    private List<GameObject> constructingUnits; //Units that are constructing the building


    private bool constructionOnGoing = false; //Indicates if a building construction is on going

    // Use this for initialization
    void Start () {

        finalMesh = GetComponent<MeshFilter>().mesh;

        constructingUnits = new List<GameObject>();
        
        phase = 0;

        timer = 30;
       
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("---------------------------------construction on going " + constructionOnGoing);
        Debug.Log(timer);
        Debug.Log("-------------------------------num de units construint " + constructingUnits.Count);

        //if (GetComponent<Unit>().getInConstruction())
        //{
            //Timer that changes the mesh of the building

            //Debug.Log(timer);
            
            timer -= constructingUnits.Count * Time.deltaTime;

            if(timer < 15 && phase==0)
            {
                GetComponent<MeshFilter>().mesh = progressMesh.GetComponent<MeshFilter>().sharedMesh;
                phase = 1;
            }

            if(timer<=0 && phase == 1)
            {
                GetComponent<MeshFilter>().mesh = finalMesh;
                foreach (var unit in constructingUnits) unit.GetComponent<Construct>().SetInConstruction(false);
                constructingUnits.Clear();
                //GetComponent<Unit>().SetInConstruction(false);
                constructionOnGoing = false;
                GameController.Instance.updateInteractable();

            }
        //}
	}

    public void startConstruction(GameObject unit)
    {
        //Change the mesh of the building to the initialMesh 
        GetComponent<MeshFilter>().mesh = initialMesh.GetComponent<MeshFilter>().sharedMesh;

        //Remove the gameObject transparency
        Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);

        //Add the unit
        constructingUnits.Add(unit);
        Debug.Log("unit added");

        //Start the construction 
        /*if(constructingUnits.Count == 1)
        {
            GetComponent<Unit>().SetInConstruction(true);
        }*/


    }

    public void deleteUnit(GameObject unit)
    {
        Debug.Log("Principi: Dins de deleteUnit " + constructingUnits.Count);
        bool a = constructingUnits.Remove(unit);
        Debug.Log(a);
        Debug.Log("Final: Dins de deleteUnit " + constructingUnits.Count);
    }

    public void setConstructionOnGoing(bool b)
    {
        constructionOnGoing = b;
    }

    public bool getConstructionOnGoing()
    {
        return constructionOnGoing;
    }
}
