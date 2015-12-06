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
    void Awake () {

        //finalMesh = GetComponent<MeshFilter>().mesh;

        constructingUnits = new List<GameObject>();
        
        phase = 0;

        timer = 30;
       
    }
	
	// Update is called once per frame
	void Update () {
       
        //if (GetComponent<Unit>().getInConstruction())
        if(constructionOnGoing)
        {
            //Timer that changes the mesh of the building
            //Debug.Log("Is revealer "+GetComponent<LOSEntity>().IsRevealer);
            //Debug.Log(timer);
            
            timer -= constructingUnits.Count * Time.deltaTime;

            if(timer < 15 && phase==0)
            {
                GetComponent<MeshFilter>().mesh = progressMesh.GetComponent<MeshFilter>().sharedMesh;
                phase = 1;
            }

            if(timer<=0 && phase == 1)
            {
				phase=2;
                GetComponent<MeshFilter>().mesh = finalMesh;
                foreach (var unit in constructingUnits) unit.GetComponent<Construct>().SetInConstruction(false);
                constructingUnits.Clear();
                //GetComponent<Unit>().SetInConstruction(false);
                constructionOnGoing = false;

                GetComponent<LOSEntity>().IsRevealer = (tag=="Ally");

                GameController.Instance.updateInteractable();
				GameController.Instance.addUnit(gameObject);
				if(GameData.winConditions.Contains (Victory.Wonder) && GetComponent<Identity>().unitType == UnitType.Wonder){
					if (gameObject.tag=="Ally") 
						GameController.Instance.hud.gameMenu.GetComponent<GameMenuBehaviour>().EndGameMenu(
							gameObject.transform.position, true, "You built a Wonder"
						);
					else if(gameObject.tag=="Enemy") 
						GameController.Instance.hud.gameMenu.GetComponent<GameMenuBehaviour>().EndGameMenu(
							gameObject.transform.position, false, "Enemy Wonder was created"
						);
				}

            }
        }
        else
        {
            if (constructingUnits.Count > 0)
            {
                foreach (var unit in constructingUnits) unit.GetComponent<Construct>().SetInConstruction(false);
                constructingUnits.Clear();
            }
        }
	}

    public void startConstruction(GameObject unit)
    {

		updateGrid upG = GetComponent<updateGrid>();
		
		if(upG != null){
			upG.isBuildingPlaced = true;
		}
        //Change the mesh of the building to the initialMesh 
        if(phase==0) GetComponent<MeshFilter>().mesh = initialMesh.GetComponent<MeshFilter>().sharedMesh;

        //Remove the gameObject transparency
        Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);

        //Add the unit
        constructingUnits.Add(unit);

        //Start the construction 
        /*if(constructingUnits.Count == 1)
        {
            GetComponent<Unit>().SetInConstruction(true);
        }*/


    }

    public void deleteUnit(GameObject unit)
    {
        bool a = constructingUnits.Remove(unit);
    }

    public void setConstructionOnGoing(bool b)
    {
        constructionOnGoing = b;
    }

    public bool getConstructionOnGoing()
    {
        return constructionOnGoing;
    }

    public void setFinalMesh()
    {
        finalMesh = GetComponent<MeshFilter>().mesh;
    }

    public GameObject getInitialMesh()
    {
        return initialMesh;
    }
	public int getPhase(){
		return phase;
	}

    public List<GameObject> getConstructingUnits()
    {
        return constructingUnits;
    }
}

