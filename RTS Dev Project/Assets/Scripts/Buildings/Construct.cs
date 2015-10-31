using UnityEngine;
using System.Collections;

public class Construct : MonoBehaviour {

    private bool inConstruction; //Indicates if a unit is constructing a building

    private bool construct; //Indicates if a unit has the order to construct a building
    
    private GameObject buildingToConstruct;

    [SerializeField] private float dist;

    public GameObject dustPrefab;
    public GameObject usingDust;


    // Use this for initialization
    void Start ()
    {
        Debug.Log("Start de construct-----------------");
        inConstruction = false;
        construct = false;

        dist = 10f;

    }
	
	// Update is called once per frame
	void Update ()
    {

        //If a unit has the order to construct and it is close enough to the building, start the construction
        if (construct)
        {
            Debug.Log("dins de construct civilian");
            if ((transform.position - buildingToConstruct.transform.position).magnitude < dist)
            {
                Debug.Log("A CONSTRUIIIIR!!!!!!!!");
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction(this.gameObject);
                construct = false;
                inConstruction = true;
                usingDust = Instantiate(dustPrefab, buildingToConstruct.transform.position, Quaternion.identity) as GameObject;
            }
        }
        if (inConstruction)
            GetComponentInParent<Animator>().SetBool("running", false);
        if (inConstruction == false && usingDust != null)
        {
            Destroy(usingDust);
        }

    }

    public void SetInConstruction(bool b)
    {
        inConstruction = b;
        Debug.Log(b);
    }

    public void setConstruct(bool b)
    {
        Debug.Log("-----------setConstruct");
        construct = b;
    }

    public void SetBuildingToConstruct(GameObject b)
    {
        Debug.Log("-----------setBuildingToConstruct");
        buildingToConstruct = b;
    }


    public bool getConstruct()
    {
        return construct;
    }

    public bool getInConstruction()
    {
        return inConstruction;
    }

    public GameObject getBuildingToConstruct()
    {
        return buildingToConstruct;
    }
}
