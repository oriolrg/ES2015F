using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
    private List<GameObject> resourcesFood;
    private List<GameObject> allCPUUnits;
    private List<GameObject> villagers;
    private GameObject urbanCenter;
    public static AI Instance { get; private set; }
    // Use this for initialization
    
    void Start () {
        allCPUUnits = new List<GameObject>();
        villagers = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addCPUUnit(go);
        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        urbanCenter = GameObject.FindGameObjectWithTag("StorageFood");
        resourcesFood.Sort((v1, v2) => (v1.transform.position - urbanCenter.transform.position).sqrMagnitude.CompareTo((v2.transform.position - urbanCenter.transform.position).sqrMagnitude));
        Invoke("createVillager", 1);
        Invoke("createVillager", 2);



    }

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;


        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        // not now!!! DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    
    public void addCPUUnit(GameObject u)
    {
        allCPUUnits.Add(u);
        
    }
    public void assignVillager(GameObject v)
    {
        
        
        allCPUUnits.Add(v);
        villagers.Add(v);        
        CollectResources c = v.GetComponentInParent<CollectResources>();
        c.goingToCollect = true;
        v.GetComponentInParent<UnitMovement>().startMovingCollect(resourcesFood[0].transform);  
        
        
    }

    private void createVillager()
    {
        
        urbanCenter.GetComponentInParent<TownCenter>().CreateUnit();
          
    }
    public void deleteResourceFood(GameObject r)
    {
        resourcesFood.Remove(r);
        foreach (GameObject vil in villagers) if (vil.GetComponentInParent<CollectResources>().targetToCollect == r.transform) reassignResourceToVillager(vil);
       

    }
    public void reassignResourceToVillager(GameObject v)
    {
        CollectResources collect = v.GetComponentInParent<CollectResources>();
        collect.targetToCollect = resourcesFood[0].transform;
        if (!collect.hasCollected)
        {
            collect.goingToCollect = true;
            v.GetComponentInParent<UnitMovement>().startMovingCollect(resourcesFood[0].transform);
        }
        
        
        
        
        
    }
}
