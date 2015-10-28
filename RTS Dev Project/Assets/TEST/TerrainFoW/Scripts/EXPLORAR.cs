using UnityEngine;
using System.Collections;

public class EXPLORAR : MonoBehaviour {

    private int opt;
    private Vector3 posicion;
    private GameObject g;

    // Use this for initialization
    void Start () {
        opt = 0;
        //Vector3 posicion = this.transform.position;
        /*
        GameObject g = GameObject.Find("Terrain");
        g.GetComponent<TerrainFoW>().ExploreArea(posicion, 50);
    
        TerrainFoW.Current.ExploreArea(posicion, 50);
    */    
    }
	
	// Update is called once per frame
	void Update () {
        if (opt == 15)
        {
            posicion = this.transform.position; // new Vector3(497.3756f, 797211, 191.3328f); //this.transform.position;
            g = GameObject.Find("Terrain");
            g.GetComponent<TerrainFoW>().ExploreArea(posicion, 50);
            //TerrainFoW.Current.ExploreArea(posicion, 20);
            opt = 0;
        }
        else
        {
            opt++;
        }
        

    
    }
}
