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
	
    public static AI Instance { get; private set; }
   

    // Use this for initialization
    
    void Start () {
        
        elaborateStrategy();
    }

    private void elaborateStrategy()
    {
        
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),2));
		tasks.Add (new Task(new Method(createTownCenter)));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),5));
		tasks.Add (new Task(new Method(createBarrac)));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createSoldier)),10));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian)),3));
		//tasks.Add (new Task(new Method(createWonder)));
        
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



        resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        resourcesMetal = new List<GameObject>(GameObject.FindGameObjectsWithTag("Metal"));
        resourcesWood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wood"));
        
    }
    void Update()
    {

        if (tasks.Count > 0)
        {
            if(tasks[0].method()){
            	tasks.RemoveAt(0);
			}
        }


		compareArmy();
		/*List<GameObject> lo = GameController.Instance.getAllEnemyArmy ();
		if(lo != null){
			GameObject o = lo[0];
			if(o != null){
				AttackController a = o.GetComponent<AttackController> ();
				if(a != null) {
					if(a.attacking_enemy == null) {

					}
				}
			}
		}*/

    }
    


    public GameObject getClosestTownCenter(GameObject c)
    { 

        float aux;
		List<GameObject> towncentresX;
		if (c.tag == "Ally") towncentresX = GameController.Instance.getAllAllyTownCentres();
		else if (c.tag == "Enemy") towncentresX = GameController.Instance.getAllEnemyTownCentres();
		else return null;
        
        
		float minDistance = 100000;
		GameObject closestTown = new GameObject();

		for (int i = 0; i < towncentresX.Count; i++)
        {
			aux = (towncentresX[i].transform.position - c.transform.position).magnitude;


            if (aux < minDistance)
            {
                minDistance = aux;
				closestTown = towncentresX[i];
            }
        }

        return closestTown;
        
        

    }

    public GameObject getClosestResource(GameObject c, Resource r){
        List<GameObject> resourcesX;
        if (r == Resource.Food) resourcesX = resourcesFood;
        else if (r == Resource.Metal) resourcesX = resourcesMetal;
        else if (r == Resource.Wood) resourcesX = resourcesWood;
        else return null;
        
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

 

    private bool createCivilian()
    {
        if (GameController.Instance.getAllAllyBuildings().Count > 0)
        {
			if(GameController.Instance.OnCreate(GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>(),UnitType.Civilian)) return true;
		}
		return false; 
    }
    private bool createWonder()
    {

		if (GameController.Instance.getAllEnemyCivilians().Count>0)
        {
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.Wonder], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(20, 0,-20), new Troop(GameController.Instance.getAllEnemyCivilians()));
			return true;
        }
		return false;
    }
	private bool createTownCenter()
	{
		if (GameController.Instance.getAllEnemyCivilians().Count > 0)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.TownCenter], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(20, 0, 0),  new Troop((List<GameObject>)GameController.Instance.getAllEnemyCivilians().GetRange(0,1)));			
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
		if (GameController.Instance.getAllEnemyCivilians().Count > 1)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.Barracs], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(0, 0, -20), new Troop((List<GameObject>)GameController.Instance.getAllEnemyCivilians().GetRange(1,1) ));
			return true;
		}
		return false;
		
	}
    public void deleteResource(GameObject r)
    {
        resourcesFood.Remove(r);
        resourcesMetal.Remove(r);
        resourcesWood.Remove(r);
		List<GameObject> civilians = new List<GameObject> ();
		civilians.AddRange(GameController.Instance.getAllAllyCivilians());
		civilians.AddRange(GameController.Instance.getAllEnemyCivilians());
        foreach (GameObject vil in civilians) if (vil.GetComponent<CollectResources>().targetObject == r) reassignResourceToCivilian(vil);
       

    }
    public void reassignResourceToCivilian(GameObject v)
    {

        CollectResources collect = v.GetComponent<CollectResources>();
        if (collect.targetObject != null)
			collect.targetObject = getClosestResource (v, (Resource)System.Enum.Parse(typeof(Resource), collect.targetObject.tag));
		else{
			collect.targetObject = getClosestResource (v, (Resource)Enum.GetValues(typeof(Resource)).GetValue((new System.Random()).Next(Enum.GetValues(typeof(Resource)).Length)));
			if(v.tag.Equals("Enemy")) collect.goingToCollect=true;
		}
		Construct construct = v.GetComponent<Construct> ();
		if (construct != null) {

			if (!construct.getInConstruction ()) {

				if(collect.targetObject!=null & collect.goingToCollect) collect.startMovingToCollect( collect.targetObject );
			}
		}else if(collect.targetObject!=null) collect.startMovingToCollect( collect.targetObject );


    }

    
   

	public void compareArmy(){

		if(GameController.Instance.getAllEnemyArmy().Count > GameController.Instance.getAllAllyArmy().Count ){
			attack();
		}

	}


	public void attack(){

		foreach (GameObject o in GameController.Instance.getAllEnemyArmy()) {
		
			AttackController a = o.GetComponent<AttackController> ();
			if (a != null) {
				a.attack (GameController.Instance.getAllAllyBuildings()[0]);
			}
		}

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
