using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AI : MonoBehaviour {
    public delegate bool Method();
    private List<Task> tasks;
    public List<string> resources;
    private List<GameObject> resourcesFood;
    private List<GameObject> resourcesMetal;
    private List<GameObject> resourcesWood;
    private List<GameObject> allCPUUnits;
    private List<GameObject> civilians;
    private List<GameObject> civiliansCPU;
    private List<GameObject> townCentersCPU;
    private List<GameObject> townCentersPlayer;
    private ResourceValueDictionary resourcesCPU;
    public static AI Instance { get; private set; }
   

    // Use this for initialization
    
    void Start () {
        
        elaborateStrategy();
    }

    private void elaborateStrategy()
    {
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),6));
		tasks.Add (new Task(new Method(createTownCenter)));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),5));
		tasks.Add (new Task(new Method(createBarrac)));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createSoldier)),10));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),3));
		tasks.Add (new Task(new Method(createWonder)));

    }

    void Awake()
    {
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		
		Instance = this;
        tasks = new List<Task>();
        resources = new List<string>(new string[] { "Food", "Metal", "Wood" });
        allCPUUnits = new List<GameObject>();
        civilians = new List<GameObject>();
        civiliansCPU = new List<GameObject>();
        townCentersCPU = new List< GameObject > ();
        townCentersPlayer = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addCPUUnit(go);
        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        resourcesMetal = new List<GameObject>(GameObject.FindGameObjectsWithTag("Metal"));
        resourcesWood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wood"));
        resourcesCPU = new ResourceValueDictionary();
        resourcesCPU[Resource.Food] = 1000;
        resourcesCPU[Resource.Wood] = 1000;
        resourcesCPU[Resource.Metal] = 1000;
        resourcesCPU[Resource.Population] = 10;
    }
    void Update()
    {

        if (tasks.Count > 0)
        {
            if(tasks[0].method()){
            	tasks.RemoveAt(0);
			}
        }
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

    public GameObject getClosestResource(GameObject c, Resource r){
        List<GameObject> resourcesX;
        if (r == Resource.Food) resourcesX = resourcesFood;
        else if (r == Resource.Metal) resourcesX = resourcesMetal;
        else if (r == Resource.Wood) resourcesX = resourcesWood;
        else resourcesX = resourcesFood;
        
        float aux;

		if(resourcesX.Count > 0){
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
        
		return null;
        

    }

    public void assignCivilian(GameObject v)
    {
        civilians.Add(v);
        if (v.tag == "Enemy") {
			civiliansCPU.Add (v);
			reassignResourceToCivilian(v);
		}

       
    }

    private bool createCivilian()
    {
        if (townCentersCPU.Count > 0)
        {
			if(GameController.Instance.OnCreate(townCentersCPU[0].GetComponent<Identity>(),UnitType.Civilian)) return true;
		}
		return false; 
    }
    private bool createWonder()
    {
        if (civiliansCPU.Count > 0)
        {
            GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[townCentersCPU[0].GetComponent<Identity>().civilization].units[UnitType.Wonder], townCentersCPU[0].transform.position + new Vector3(20, 0,-20), new Troop(civiliansCPU));

			return true;
        }
		return false;
    }
	private bool createTownCenter()
	{
		if (civiliansCPU.Count > 0)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[townCentersCPU[0].GetComponent<Identity>().civilization].units[UnitType.TownCenter], townCentersCPU[0].transform.position + new Vector3(20, 0, 0),  new Troop((List<GameObject>)civiliansCPU.GetRange(0,1)));			
			return true;
		}
		return false;
	}
	public bool createSoldier()
	{
		bool done = false;
		int i = 0;
		List<GameObject> buildings = GameController.Instance.getAllEnemyBuildings();
		
		foreach (GameObject b in buildings){
			if(b.GetComponent<Identity>().unitType == UnitType.Barracs & b.tag=="Enemy"){
				if(GameController.Instance.OnCreate(b.GetComponent<Identity>(), UnitType.Soldier)) return true;
			}
		}
		return false;
	}
	
	private bool createBarrac()
	{
		if (civiliansCPU.Count > 0)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[townCentersCPU[0].GetComponent<Identity>().civilization].units[UnitType.Barracs], townCentersCPU[0].transform.position + new Vector3(0, 0, -20), new Troop((List<GameObject>)civiliansCPU.GetRange(1,1) ));
			return true;
		}
		return false;
		
	}
    public void deleteResource(GameObject r)
    {
        resourcesFood.Remove(r);
        resourcesMetal.Remove(r);
        resourcesWood.Remove(r);
        foreach (GameObject vil in civilians) if (vil.GetComponent<CollectResources>().targetObject == r) reassignResourceToCivilian(vil);
       

    }
    public void reassignResourceToCivilian(GameObject v)
    {

        CollectResources collect = v.GetComponent<CollectResources>();
        if (collect.targetObject != null)
			collect.targetObject = getClosestResource (v, (Resource)System.Enum.Parse(typeof(Resource), collect.targetObject.tag));
		else
			collect.targetObject = getClosestResource (v, (Resource)Enum.GetValues(typeof(Resource)).GetValue((new System.Random()).Next(Enum.GetValues(typeof(Resource)).Length)));
		Construct construct = v.GetComponent<Construct> ();
		if (construct != null) {

			if (!construct.getInConstruction ()) {

				if(collect.targetObject!=null) collect.startMovingToCollect( collect.targetObject );
			}
		}else if(collect.targetObject!=null) collect.startMovingToCollect( collect.targetObject );


    }

    public bool checkResources(ResourceValueDictionary resourceCosts)
    {
        bool check = true;
        foreach (KeyValuePair<Resource, int> kv in resourceCosts)
        {
            if (resourcesCPU[kv.Key] - kv.Value < 0)
            {
                //Here goes stuff that happens when there aren't enough resources to perform the action.
                //i.e. text pop-up, sound warning.
                check = false;
            }
        }        
        return check;
    }
    public void updateResource(Resource res, int value)
    {
        resourcesCPU[res] -= value;
    }

	public bool compareArmy(){

		//return true;
		return GameController.Instance.getAllEnemyArmy().Count > GameController.Instance.getAllAllyArmy().Count ;

	}



    public class Task
    {
        public Method method;
        public Task(Method m)
        {
            method = m;
        }

    }
}
