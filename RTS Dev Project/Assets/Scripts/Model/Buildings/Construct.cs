using UnityEngine;
using System.Collections;

public class Construct : MonoBehaviour {

	[SerializeField]private bool inConstruction; //Indicates if a unit is constructing a building

	[SerializeField]private bool construct; //Indicates if a unit has the order to construct a building
    
    private GameObject buildingToConstruct;

	private float dist;

    public GameObject dustPrefab;
    public GameObject usingDust;


    // Use this for initialization
    void Start ()
    {
        inConstruction = false;
        construct = false;

        //dist = 10f;

    }
	
	// Update is called once per frame
	void Update ()
    {

        //If a unit has the order to construct and it is close enough to the building, start the construction
        if (construct)
        {

            if ((transform.position - buildingToConstruct.transform.position).magnitude < dist)
            {
			
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction(this.gameObject);
                
                construct = false;
                inConstruction = true;
                if(usingDust == null) usingDust = Instantiate(dustPrefab, buildingToConstruct.transform.position, Quaternion.identity) as GameObject;

                UnitMovement uM = gameObject.GetComponent<UnitMovement>();
                if (uM != null)
                {
                    uM.hasTarget = false;
                    Animator a = GetComponentInParent<Animator>();
                    a.SetBool("walk", false);
                    //uM.status = Status.idle;
                }
            }

        }
        if (inConstruction)
            GetComponentInParent<Animator>().SetBool("chop", true);

        if (inConstruction == false && usingDust != null)
        {
            
            Destroy(usingDust);
            usingDust = null;
            GetComponentInParent<Animator>().SetBool("chop", false);
        }

    }

    public void SetInConstruction(bool b)
    {
        inConstruction = b;
        Debug.Log(b);
    }

    public void setConstruct(bool b)
    {
        construct = b;
    }

    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;

        Collider c = buildingToConstruct.GetComponent<Collider>();
        if (c != null)
        {
            dist = c.bounds.extents.magnitude;//.size.magnitude / 2;
        }
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
