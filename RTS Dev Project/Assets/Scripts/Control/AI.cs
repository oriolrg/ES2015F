using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
    private List<GameObject> resourcesFood;
    private List<GameObject> allCPUUnits;
    private List<GameObject> civilians;
    private GameObject urbanCenter;
    public static AI Instance { get; private set; }
    // Use this for initialization
    
    void Start () {
        allCPUUnits = new List<GameObject>();
        civilians = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addCPUUnit(go);
        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        urbanCenter = GameObject.FindGameObjectWithTag("StorageFood");
        resourcesFood.Sort((v1, v2) => (v1.transform.position - urbanCenter.transform.position).sqrMagnitude.CompareTo((v2.transform.position - urbanCenter.transform.position).sqrMagnitude));
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

    public void assignCivilian(GameObject v)
    {
        allCPUUnits.Add(v);
        civilians.Add(v);        
        CollectResources c = v.GetComponentInParent<CollectResources>();
        c.goingToCollect = true;
        v.GetComponentInParent<UnitMovement>().startMovingCollect(resourcesFood[0].transform);  
        
        
    }

    private void createCivilian()
    {
        
        urbanCenter.GetComponentInParent<TownCenter>().CreateCivilian();
          
    }
    public void deleteResourceFood(GameObject r)
    {
        resourcesFood.Remove(r);
        foreach (GameObject vil in civilians) if (vil.GetComponentInParent<CollectResources>().targetToCollect == r.transform) reassignResourceToCivilian(vil);
       

    }
    public void reassignResourceToCivilian(GameObject v)
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
