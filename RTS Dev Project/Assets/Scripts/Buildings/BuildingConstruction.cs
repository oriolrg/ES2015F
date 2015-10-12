using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BuildingConstruction : MonoBehaviour {

    public GameObject initialMesh;
    public GameObject progressMesh;
    private Mesh finalMesh;
    private bool inConstruction;
    public float timer = 30;
    private int phase;
    private int numUnitsConstructing;
    private List<GameObject> constructingUnits;

    // Use this for initialization
    void Start () {
        finalMesh = GetComponent<MeshFilter>().mesh;

        inConstruction = false;
        numUnitsConstructing = 0;
        constructingUnits = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(timer);

        if (inConstruction)
        {
            Debug.Log(timer);
            Debug.Log(numUnitsConstructing);
            timer -= numUnitsConstructing * Time.deltaTime;

            if(timer < 15 && phase==0)
            {
                GetComponent<MeshFilter>().mesh = progressMesh.GetComponent<MeshFilter>().sharedMesh;
                phase = 1;
            }

            if(timer<=0 && phase == 1)
            {
                GetComponent<MeshFilter>().mesh = finalMesh;
                inConstruction = false;
                numUnitsConstructing = 0;
                foreach (var unit in constructingUnits) unit.GetComponent<Focusable>().SetInConstruction(false);
                constructingUnits.Clear();
                GetComponent<Focusable>().SetInConstruction(false);

            }
        }
	}

    public void startConstruction(GameObject unit)
    {
        GetComponent<MeshFilter>().mesh = initialMesh.GetComponent<MeshFilter>().sharedMesh;

        Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);

        constructingUnits.Add(unit);
        numUnitsConstructing += 1;

        if(numUnitsConstructing == 1)
        {
            inConstruction = true;
            phase = 0;

        }

        GetComponent<Focusable>().SetInConstruction(true);

    }
}
