using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AI : MonoBehaviour {
    public delegate void Method();
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
        Task t = new Task(new Method(createCivilian));
        tasks.AddRange(Enumerable.Repeat(t,6));
        t = new Task(new Method(createWonder));

		tasks.Add (t);

    }

    void Awake()
    {
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        resourcesCPU[Resource.Food] = 1000;
        resourcesCPU[Resource.Wood] = 1000;
        resourcesCPU[Resource.Metal] = 1000;
        resourcesCPU[Resource.Population] = 10;
    }
    void Update()
    {

        if (tasks.Count > 0)
        {
			print (tasks[0].method.Method.Name);
            tasks[0].method();
            tasks.RemoveAt(0);
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
        civilians.Add(v);
        if (v.tag == "Enemy") civiliansCPU.Add(v);    

       
    }

    private void createCivilian()
    {
        if (townCentersCPU.Count > 0)
        {
			GameController.Instance.OnCreate(townCentersCPU[0].GetComponent<Identity>(),UnitType.Civilian);
			//GameController.Instance.CreateUnit(townCentersCPU[0], DataManager.Instance.civilizationDatas[townCentersCPU[0].GetComponent<Identity>().civilization].units[UnitType.Civilian]);
        }
    }
    private void createWonder()
    {
        if (civiliansCPU.Count > 0)
        {

            GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[townCentersCPU[0].GetComponent<Identity>().civilization].units[UnitType.TownCenter], townCentersCPU[0].transform.position + new Vector3(20, 0, 20), new Troop(civiliansCPU));
			//GameController.Instance.OnCreate(civiliansCPU[0].GetComponent<Identity>(),UnitType.TownCenter);
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
        if (collect.targetToCollect != null) collect.targetToCollect = getClosestResource(v, collect.targetToCollect.tag);
        ///PLEASE CHECK THIS IS AI WORK
        else collect.targetToCollect = getClosestResource(v, "Wood");

        collect.startMovingToCollect(collect.targetToCollect);

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

	public void createSoldier()
	{
		bool done = false;
		int i = 0;
		List<GameObject> buildings = GameController.Instance.getAllEnemyBuildings();
		
		while(!done)
		{
			GameObject o = buildings[i];
			if(o.GetComponent<Identity>().unitType == UnitType.Barracs){
				GameController.Instance.OnCreate(o.GetComponent<Identity>(), UnitType.Soldier);
				done = true;
			}
			i = i  + 1; 
		}


		//GameController.Instance.hud.getActionsData().creationPermissions[UnitType.Soldier]);
	}

	private void createBarrac()
	{//UnitType.Civilian
		//GameController.Instance.OnCreate(o.GetComponent<Identity>(), UnitType.Barracs);
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
