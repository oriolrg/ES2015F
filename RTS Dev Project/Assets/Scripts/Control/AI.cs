using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
    private List<GameObject> resourcesFood;
    private List<GameObject> allCPUUnits;
    private List<GameObject> civilians;
    private List<GameObject> townCentersCPU;
    private List<GameObject> townCentersPlayer;
    public static AI Instance { get; private set; }
    // Use this for initialization
    
    void Start () {
        allCPUUnits = new List<GameObject>();
        civilians = new List<GameObject>();
        townCentersCPU = new List< GameObject > ();
        townCentersPlayer = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addCPUUnit(go);
        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        //resourcesFood.Sort((v1, v2) => (v1.transform.position - townCentersCPU[0].transform.position).sqrMagnitude.CompareTo((v2.transform.position - townCentersCPU[0].transform.position).sqrMagnitude));
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
            resourcesFood.Sort((v1, v2) => (v1.transform.position - townCentersCPU[0].transform.position).sqrMagnitude.CompareTo((v2.transform.position - townCentersCPU[0].transform.position).sqrMagnitude));
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
    public GameObject getClosestFood(GameObject c){
        float aux;
        float minDistance = (resourcesFood[0].transform.position - c.transform.position).magnitude;
        GameObject closestFood = resourcesFood[0];

        for (int i = 0; i < resourcesFood.Count - 1; i++)
        {
            aux = (resourcesFood[i].transform.position - c.transform.position).magnitude;
            if (aux < minDistance)
            {
                minDistance = aux;
                closestFood = resourcesFood[i];
            }
        }
        return closestFood;

    }

    public void assignCivilian(GameObject v)
    {
        allCPUUnits.Add(v);
        civilians.Add(v);        
        //CollectResources c = v.GetComponentInParent<CollectResources>();
        //c.targetToCollect = resourcesFood[0];
        //c.startMovingToCollect(resourcesFood[0]);  
       
    }

    private void createCivilian()
    {
        // !!! 
        //townCentersCPU[0].GetComponentInParent<TownCenter>().CreateCivilian();
          
    }
    public void deleteResourceFood(GameObject r)
    {
        resourcesFood.Remove(r);
        foreach (GameObject vil in civilians) if (vil.GetComponent<CollectResources>().targetToCollect == r) reassignResourceToCivilian(vil);
       

    }
    public void reassignResourceToCivilian(GameObject v)
    {
        CollectResources collect = v.GetComponentInParent<CollectResources>();
        collect.targetToCollect = getClosestFood(v);
        if (!collect.hasCollected)
        {
            collect.startMovingToCollect(collect.targetToCollect);
        }
    }
}
