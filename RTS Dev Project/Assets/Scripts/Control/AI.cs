using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class AI : MonoBehaviour {
	public delegate bool Method(UnitType u);
	private List<Task> tasks;
	public List<string> resources;
	private List<GameObject> resourcesFood;
	private List<GameObject> resourcesMetal;
	private List<GameObject> resourcesWood;
	private float inf = 99999999;
	
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
	private void elaborateStrategyObjectives()
	{
		List<GameObject> civ = GameController.Instance.getAllEnemyCivilians ();
		for (int i = 0; i<civ.Count; i++) {
			Boolean isbusy=false;
			foreach(Objective obj in GameController.Instance.objectives) if(civ[i].GetComponent<UnitMovement>().target == obj.transform) isbusy=true;
			if(!isbusy)
				civ[i].GetComponent<UnitMovement>().startMoving(GameController.Instance.objectives[i%GameController.Instance.objectives.Count].gameObject);
		}
	}
	private void elaborateStrategyWonder()
	{
		tasks = new List<Task>();
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
		
		
		
		resourcesFood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
		resourcesMetal = new List<GameObject>(GameObject.FindGameObjectsWithTag("Metal"));
		resourcesWood = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wood"));
		
	}
	void Update()
	{
		if (GameData.diff == GameData.DifficultyEnum.Medium || GameData.diff == GameData.DifficultyEnum.Hard) {	 
			float evalWinObjective = evaluateWinByObjectives ();
			float evalWinWonder = evaluateWinByWonder ();
			if (evalWinObjective > evalWinWonder && evalWinObjective < 1000) //&&  Victory.MapControl in GameData.winConditions   
				elaborateStrategyObjectives ();
			else if (evalWinWonder < 1000) // && Victory.Wonder in GameData.winConditions 
				elaborateStrategyWonder ();

			if(GameData.diff == GameData.DifficultyEnum.Hard){
				//codi carme
			}
		}
		
		if (tasks.Count > 0) {
			if (tasks [0].method ()) {
				tasks.RemoveAt (0);
			}
		}
		compareArmy();

		
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


	
	
	private bool createCivilian(UnitType u)
	{
		if (GameController.Instance.getAllEnemyTownCentres().Count > 0)
		{
			if(GameController.Instance.OnCreate(GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>(),UnitType.Civilian)) return true;
		}
		return false; 
	}
	private bool createWonder(UnitType u)
	{
		
		if (GameController.Instance.getAllEnemyCivilians().Count>0)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.Wonder], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(20, 0,-20), new Troop(GameController.Instance.getAllEnemyCivilians()));
			return true;
		}
		return false;
	}
	private bool createTownCenter(UnitType u)
	{
		if (GameController.Instance.getAllEnemyCivilians().Count > 0)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.TownCenter], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(20, 0, 0),  new Troop((List<GameObject>)GameController.Instance.getAllEnemyCivilians().GetRange(0,1)));			
			return true;
		}
		return false;
	}
	public bool createSoldier(UnitType u)
	{

		List<GameObject> buildings = GameController.Instance.getAllEnemyBuildings();
		
		foreach (GameObject b in buildings){
			if(b.GetComponent<Identity>().unitType == UnitType.Barracs){
				if(GameController.Instance.OnCreate(b.GetComponent<Identity>(), UnitType.Soldier)) return true;
			}
		}
		if(! isCPUBuilding (UnitType.Barracs))
			tasks.Insert(0, new Task(new Method(createBarrac)));
		return false;
	}
	
	private bool createBarrac(UnitType u)
	{
		if (GameController.Instance.getAllEnemyCivilians().Count > 1)
		{
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[UnitType.Barracs], GameController.Instance.getAllEnemyTownCentres()[0].transform.position + new Vector3(0, 0, -20), new Troop((List<GameObject>)GameController.Instance.getAllEnemyCivilians().GetRange(1,1) ));
			return true;
		}
		return false;
		
	}

	public bool createArcher(UnitType u)
	{


		List<GameObject> buildings = GameController.Instance.getAllEnemyBuildings();
		
		foreach (GameObject b in buildings){
			if(b.GetComponent<Identity>().unitType == UnitType.Archery){
				if(GameController.Instance.OnCreate(b.GetComponent<Identity>(), UnitType.Archer)) return true;
			}
		}
		if(! isCPUBuilding (UnitType.Archery))
			tasks.Insert(0, new Task(new Method(createBuilding(UnitType.Archery))));
     	return false;
     }

	private bool createBuilding(UnitType u)
	{
		GameObject civil = getIdleCivilian ();
		if (civil == null)
		{
			Vector3 v = new Vector3(UnityEngine.Random.Range(20,40), 0, UnityEngine.Random.Range(20,40));
			
			float position = civil.transform.position + v;
			if(GameController.Instance.getAllEnemyTownCentres().Count > 0)
				position = 	GameController.Instance.getAllEnemyTownCentres()[0] + v;
			GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[u], position, new Troop( new List<GameObject>(){civil}));
			return true;
		}
		return false;
		
	}


	public GameObject getIdleCivilian(){
	
		foreach (GameObject o in GameController.Instance.getAllEnemyCivilians()) {
			Construct c = o.GetComponent<Construct> (); 
			if (c != null) {
				if (! c.getInConstruction () & !c.getConstruct ()) {
					
						return c;
				}
			}
		}
		return null;
	}



	public bool isCPUBuilding (UnitType u){
		

		foreach (GameObject o in GameController.Instance.getAllEnemyCivilians()) {
			Construct c = o.GetComponent<Construct> (); 
			if (c != null) {
				if (c.getInConstruction () || c.getConstruct ()) {
					GameObject build = c.getBuildingToConstruct ();
					if (build.GetComponentOrEnd<Identity> ().unitType == u) {
						return true;
					}
				}
			}
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
	
	public float evaluateWinByObjectives(){
		List<Objective> obs = GameController.Instance.objectives;
		float time = inf;
		if (obs.Count > 0) {
			time=0;
			float meanDistancesObjectives=0;
			foreach (Objective objective in obs){
				meanDistancesObjectives+=(GameController.Instance.getAllEnemyTownCentres()[0].transform.position - objective.transform.position).magnitude;
			}
			meanDistancesObjectives/=obs.Count;
			time += Math.Max(1,obs.Count-GameController.Instance.getAllEnemyCivilians().Count)*meanDistancesObjectives/DataManager.Instance.unitDatas[UnitType.Civilian].stats[Stat.Speed];
			
		} 
		return time;
	} 
	public float evaluateWinByWonder(){
		float time = 0;
		if (GameController.Instance.getAllEnemyCivilians ().Count > 0)
			time += DataManager.Instance.unitDatas [UnitType.Wonder].requiredTime / GameController.Instance.getAllEnemyCivilians ().Count;
		else
			return inf;
		float resourcesNeeded = 0;
		foreach (KeyValuePair<Resource, int> kv in DataManager.Instance.unitDatas[UnitType.Wonder].resourceCost)
		{
			resourcesNeeded+= Math.Max (0, kv.Value-GameController.Instance.getCPUResources()[kv.Key]) ;
		}
		time = resourcesNeeded * 3;//Now make a way to go from number of resources needed to time it takes to harvest it
		return time;
	}
	
	
	
	



	public void counterattack(GameObject target){
		
		List<GameObject> allyAtacking = whoIsAttacking (target);
		List<GameObject> enemiesNoAtacking = getEnemiesNoAtacking(allyAtacking.Count);
		
		
		print ("Enemies noAttacking "+ enemiesNoAtacking.Count);
		
		print ("Player " + allyAtacking.Count);
		
		/*int numEnemiesPerPlayer = 1;
		 
		if (enemiesNoAtacking.Count == allyAtacking.Count) {
			numEnemiesPerPlayer = 1;
		} else if ((enemiesNoAtacking.Count) >= (allyAtacking.Count) * 2) {
			numEnemiesPerPlayer = 2;
		} else if ( (allyAtacking.Count) <  (enemiesNoAtacking.Count) < (allyAtacking.Count) * 2) {
			numEnemiesPerPlayer = 1;
		}*/
		
		int count = 0;
		int playerUnit = 0;
		for(int i = 0; i < Math.Min (allyAtacking.Count + 1, enemiesNoAtacking.Count) ; i++){
			//foreach (GameObject o in enemiesNoAtacking) {
			GameObject o = enemiesNoAtacking[i];
			
			AttackController a = o.GetComponent<AttackController> ();
			if (a != null) {
				if(i < allyAtacking.Count){
					a.attack(allyAtacking[i]);
				}else{
					a.attack(allyAtacking[0]);
				}
			}
			
		}
	}
	
	
	
	public List<GameObject>  whoIsAttacking(GameObject target){
		
		List<GameObject> lo = new List<GameObject>();
		
		foreach (GameObject o in GameController.Instance.getAllAllyArmy()) {
			
			AttackController a = o.GetComponent<AttackController> ();
			if (a != null) {
				if(a.attacking_enemy == target){
					lo.Add(o);
					
				}
			}
		}
		
		foreach (GameObject o in GameController.Instance.getAllAllyCivilians()) {
			
			AttackController a = o.GetComponent<AttackController> ();
			if (a != null) {
				if (a.attacking_enemy == target) {
					lo.Add (o);
					
				}
			}
		}
		
		return lo;
		
	}
	
	
	
	public List<GameObject> getEnemiesNoAtacking(int numTargets){
		
		
		List<GameObject> enemiesNotBusy = new List<GameObject>();
		bool noAttacking = true;
		bool noConstructing = true;
		foreach (GameObject o in GameController.Instance.getAllEnemyArmy()) {
			
			AttackController a = o.GetComponent<AttackController> ();
			
			if (a != null) {
				if(a.attacking_enemy  == null){
					
					enemiesNotBusy.Add(o);
					
				}
			}
		}
		if (enemiesNotBusy.Count < numTargets) {
			foreach (GameObject o in GameController.Instance.getAllEnemyCivilians()) {
				noAttacking = true;
				noConstructing = true;
				
				AttackController a = o.GetComponent<AttackController> ();
				if (a != null) {
					if(a.attacking_enemy == null){
						noAttacking = false;
						
					}
				}
				
				Construct c = o.GetComponent<Construct> (); 
				if(c != null){
					if(c.getInConstruction()){
						
						noConstructing = false;
					}
				}
				if(noAttacking && noConstructing){
				enemiesNotBusy.Add(o);
				}
				
			}
		}
		
		return enemiesNotBusy;
	}


	



	public void counterAttackWonder(){
		
		GameObject wonder = isPlayerBuildingWonder ();
		if(wonder != null){
			for(int i = 0; i < 5; i++){
				GameObject o = GameController.Instance.getAllEnemyArmy()[i];
				
				AttackController a = o.GetComponent<AttackController> ();
				if (a != null) {
					a.attack (wonder);
				}
			}
			
		}
		
	}


	public GameObject isPlayerBuildingWonder (){

		foreach (GameObject o in GameController.Instance.getAllAllyCivilians()) {
			Construct c = o.GetComponent<Construct> (); 
			if(c != null){
				if(c.getInConstruction()){
					GameObject build = c.getBuildingToConstruct();
					if(build.GetComponentOrEnd<Identity>().unitType == UnitType.Wonder){
						return build;
					}
				}
			}
		}
		return null;
	}
	
	
	public void counterAttackAnnihilation(){

		/*int numTownCenter = 0;
		int numBarracks = 0;
		int numStable = 0;
		int numArchery = 0;
		foreach (GameObject o in GameController.Instance.getAllEnemyBuildings()) {
			switch (o.GetComponent<Identity>().unitType ){
			case UnitType.TownCenter:
				numTownCenter += 1;
				break;
			
			/*case UnitType.Barracs:
				numBarracks += 1;
				break;

			case UnitType.Stable:
				numStable += 1;
				break;

			case UnitType.Archery:
				numArchery += 1;
				break;
			default:
				print (o.GetComponent<Identity>().unitType );
				break;
			}
			
		}

		if(numTownCenter < 0) {
			GameController.Instance.hud.showMessageBox("CPU must to create a townCenter");
		}else if(numBarracks< 2){
			GameController.Instance.hud.showMessageBox("CPU must to create a Barrack");
		}else if(numStable< 2){
			GameController.Instance.hud.showMessageBox("CPU must to create a Stable");
		}else if(numArchery< 2){
			GameController.Instance.hud.showMessageBox("CPU must to create a Archery");
		}*/

		if(GameController.Instance.getAllEnemyTownCentres().Count == 0)
			tasks.Insert(0, new Task(new Method(createTownCenter)));
		
	
	}

	public class Task
	{
		public Method method;
		public Task(Method m){
			method = m;
		}
		
	}

}

