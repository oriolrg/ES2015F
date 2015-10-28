using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BuildingConstruction : MonoBehaviour {

    //The different meshes of the building during the construction
    public GameObject initialMesh;
    public GameObject progressMesh;
    private Mesh finalMesh;

    //private bool inConstruction;
    public float timer = 30; //Timer that changes the mesh
    private int phase; //Phase of the construction
    private List<GameObject> constructingUnits; //Units that are constructing the building

    // Use this for initialization
    void Start () {

        finalMesh = GetComponent<MeshFilter>().mesh;

        constructingUnits = new List<GameObject>();

        phase = 0;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(timer);

        
        if (GetComponent<Unit>().getInConstruction())
        {
            //Timer that changes the mesh of the building

            //Debug.Log(timer);
            //Debug.Log(constructingUnits.Count);
            timer -= constructingUnits.Count * Time.deltaTime;

            if(timer < 15 && phase==0)
            {
                GetComponent<MeshFilter>().mesh = progressMesh.GetComponent<MeshFilter>().sharedMesh;
                phase = 1;
            }

            if(timer<=0 && phase == 1)
            {
                GetComponent<MeshFilter>().mesh = finalMesh;
                foreach (var unit in constructingUnits) unit.GetComponent<Unit>().SetInConstruction(false);
                constructingUnits.Clear();
                GetComponent<Unit>().SetInConstruction(false);

            }
        }
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

        //Start the construction 
        if(constructingUnits.Count == 1)
        {
            GetComponent<Unit>().SetInConstruction(true);
        }


    }
}
