using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
    public List<string> resources;
    private List<GameObject> resourcesFood;
    private List<GameObject> resourcesMetal;
    private List<GameObject> resourcesWood;
    private List<GameObject> allCPUUnits;
    private List<GameObject> civilians;
    private List<GameObject> townCentersCPU;
    private List<GameObject> townCentersPlayer;
    public static AI Instance { get; private set; }

    // Use this for initialization
    
    void Start () {
        resources = new List<string>(new string[] { "Food", "Metal", "Wood" });
        allCPUUnits = new List<GameObject>();
        civilians = new List<GameObject>();
        townCentersCPU = new List< GameObject > ();
        townCentersPlayer = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addCPUUnit(go);
        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        resourcesMetal = new List<GameObject>(GameObject.FindGameObjectsWithTag("Metal"));
        resourcesWood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wood"));
        Invoke("createCivilian", 2);
        Invoke("createCivilian", 4);
        Invoke("createCivilian", 6);
        Invoke("createCivilian", 8);
        Invoke("createCivilian", 10);



    }  

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    
    public void addCPUUnit(GameObject u)
    {
        allCPUUnits.Add(u);
        
    }
    public void addTownCenter(GameObject t)
    {
        if (t.tag == "Ally")
        {
            townCentersPlayer.Add(t);
        }
        else if (t.tag == "Enemy")
        {
            townCentersCPU.Add(t);
        }

    }
    public GameObject getClosestTownCenter(GameObject c)
    {
        float aux;
        /*
        GameObject[] auxList = new GameObject[100];
        if (c.tag == "Ally")
        {
            townCentersPlayer.CopyTo(auxList);
        }
        else if (c.tag == "Enemy")
        {
            townCentersCPU.CopyTo(auxList);
        }*/
        if (c.tag == "Enemy")
        {
            float minDistance = (townCentersCPU[0].transform.position - c.transform.position).magnitude;
            GameObject closestTown = townCentersCPU[0];

            for (int i = 0; i < townCentersCPU.Count - 1; i++)
            {
                aux = (townCentersCPU[i].transform.position - c.transform.position).magnitude;
                if (aux < minDistance)
                {
                    minDistance = aux;
                    closestTown = townCentersCPU[i];
                }
            }
            return closestTown;
        } else if(c.tag == "Ally")
        {
            float minDistance = (townCentersPlayer[0].transform.position - c.transform.position).magnitude;
            GameObject closestTown = townCentersPlayer[0];

            for (int i = 0; i < townCentersPlayer.Count - 1; i++)
            {
                aux = (townCentersPlayer[i].transform.position - c.transform.position).magnitude;
                if (aux < minDistance)
                {
                    minDistance = aux;
                    closestTown = townCentersPlayer[i];
                }
            }
            return closestTown;
        }
        else { return null; }
    }
    public GameObject getClosestResource(GameObject c, string r){
        List<GameObject> resourcesX;
        if (r == "Food") resourcesX = resourcesFood;
        else if (r == "Metal") resourcesX = resourcesMetal;
        else if (r == "Wood") resourcesX = resourcesWood;
        else resourcesX = resourcesFood;
        
        float aux;
        float minDistance = (resourcesX[0].transform.position - c.transform.position).magnitude;
        GameObject closestResource = resourcesX[0];

        for (int i = 0; i < resourcesX.Count; i++)
        {
            aux = (resourcesX[i].transform.position - c.transform.position).magnitude;
            if (aux < minDistance)
            {
                minDistance = aux;
                closestResource = resourcesX[i];
            }
        }
        return closestResource;

    }

    public void assignCivilian(GameObject v)
    {
        allCPUUnits.Add(v);
        civilians.Add(v);        

       
    }

    private void createCivilian()
    {
        if (townCentersCPU.Count > 0)
        {
            townCentersCPU[0].GetComponent<TownCenter>().CreateCivilian();
        }
        
          
    }
    public void deleteResource(GameObject r)
    {
        resourcesFood.Remove(r);
        resourcesMetal.Remove(r);
        resourcesWood.Remove(r);
        foreach (GameObject vil in civilians) if (vil.GetComponent<CollectResources>().targetToCollect == r) reassignResourceToCivilian(vil);
       

    }
    public void reassignResourceToCivilian(GameObject v)
    {
        CollectResources collect = v.GetComponent<CollectResources>();
        if (collect.targetToCollect!=null) collect.targetToCollect = getClosestResource(v,collect.targetToCollect.tag);
        ///PLEASE CHECK THIS IS AI WORK
        else collect.targetToCollect = getClosestResource(v, "Metal");
        if (collect.goingToCollect) collect.startMovingToCollect(collect.targetToCollect);

    }
}
