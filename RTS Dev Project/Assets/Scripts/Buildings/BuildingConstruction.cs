using UnityEngine;
using System.Collections;

public class BuildingConstruction : MonoBehaviour {

    public GameObject initialMesh;
    public GameObject progressMesh;
    private Mesh finalMesh;
    private bool inConstruction;
    public float timer = 30;
    private int phase;

    // Use this for initialization
    void Start () {
        finalMesh = GetComponent<MeshFilter>().mesh;

        inConstruction = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (inConstruction)
        {
            timer -= Time.deltaTime;

            if(timer < 15 && phase==0)
            {
                GetComponent<MeshFilter>().mesh = progressMesh.GetComponent<MeshFilter>().sharedMesh;
                phase = 1;
            }

            if(timer<=0 && phase == 1)
            {
                GetComponent<MeshFilter>().mesh = finalMesh;
                inConstruction = false;
            }
        }
	}

    public void startConstruction()
    {
        GetComponent<MeshFilter>().mesh = initialMesh.GetComponent<MeshFilter>().sharedMesh;

        Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);

        inConstruction = true;
        phase = 0;

    }
}
