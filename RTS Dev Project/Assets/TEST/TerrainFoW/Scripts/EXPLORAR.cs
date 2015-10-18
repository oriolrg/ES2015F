using UnityEngine;
using System.Collections;

public class EXPLORAR : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Vector3 posicion = this.transform.position;
        /*
        GameObject g = GameObject.Find("Terrain");
        g.GetComponent<TerrainFoW>().ExploreArea(posicion, 50);
    
        TerrainFoW.Current.ExploreArea(posicion, 50);
    */    
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector3 posicion = this.transform.position; // new Vector3(497.3756f, 797211, 191.3328f); //this.transform.position;
        TerrainFoW.Current.ExploreArea(posicion, 20);
    
    }
}
