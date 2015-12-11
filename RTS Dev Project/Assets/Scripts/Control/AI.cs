using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class AI : MonoBehaviour {
	public delegate bool Method(UnitType u);
	[SerializeField] private GameObject targetPrefab;
	private List<Task> tasks;
	public List<string> resources;
	private List<GameObject> resourcesFood;
	private List<GameObject> resourcesMetal;
	private List<GameObject> resourcesWood;
	private float inf = 99999999;
	private bool contructingBarrack = false;
	private bool counterAttackingObjectives = false;
	private bool counterAttackingWonder = false;
	public static AI Instance { get; private set; }
	
	
	// Use this for initialization
	
	void Start () {
		
		elaborateStrategy();
	}
	
	private void elaborateStrategy()
	{
		
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian), UnitType.Civilian ),1));
		tasks.Add (new Task(new Method(createBarrac), UnitType.Barracs));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createSoldier), UnitType.Soldier),10));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian), UnitType.Civilian),5));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createArcher), UnitType.Archer),20));
		tasks.AddRange(Enumerable.Repeat(new Task(new Method(createCivilian), UnitType.Civilian),5));
		//tasks.Add (new Task(new Method(createWonder)));
		
	}
	private void elaborateStrategyObjectives()
	{
		print ("WIN BY OBJECTIVES");
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
		if (!isCPUBuilding (UnitType.Wonder)&!tasks.Contains( new Task (new Method (createBuilding), UnitType.Wonder))) {
			print ("WIN BY WONDER");
			//tasks.Insert(0,new Task (new Method (createBuilding), UnitType.Wonder));
		}
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
		GameController.Instance.hud.updateResourceAI(Resource.Population, GameController.Instance.getAllEnemyCivilians().Count + GameController.Instance.getAllEnemyArmy().Count);
		GameController.Instance.hud.updateResourceAI(Resource.Buildings, GameController.Instance.getAllEnemyBuildings().Count);
		GameController.Instance.hud.updateResourceAI(Resource.Wood, GameController.Instance.cpuResources[Resource.Wood]);
		GameController.Instance.hud.updateResourceAI(Resource.Food, GameController.Instance.cpuResources[Resource.Food]);
		GameController.Instance.hud.updateResourceAI(Resource.Metal, GameController.Instance.cpuResources[Resource.Metal]);


		if (GameData.cpus[0].skill==GameData.DifficultyEnum.Medium|| GameData.cpus[0].skill==GameData.DifficultyEnum.Hard) {	 
			float evalWinObjective = evaluateWinByObjectives ();
			float evalWinWonder = evaluateWinByWonder ();
			if (evalWinObjective < evalWinWonder && evalWinObjective < 40){ //&&  Victory.MapControl in GameData.winConditions   
				print ("Evaluacio win by objective " + evalWinObjective);
				elaborateStrategyObjectives ();
			}
			else if (evalWinWonder < 50) // && Victory.Wonder in GameData.winConditions 
				elaborateStrategyWonder ();

			if(GameData.cpus[0].skill==GameData.DifficultyEnum.Hard){

				bool counterAttackAnnihilationDone = false;
				bool counterAttackWonderDone = false;
				if(GameData.winConditions.Contains(Victory.MapControl ))
					counterAttackAnnihilationDone = counterAttackMapControl();


				if(!counterAttackAnnihilationDone & GameData.winConditions.Contains(Victory.Wonder))
					counterAttackWonderDone = counterAttackWonder();
				if(!counterAttackAnnihilationDone & !counterAttackWonderDone & GameData.winConditions.Contains(Victory.Annihilation))
					counterAttackAnnihilation();
			}
		}


		if (tasks.Count > 0) {
			if (tasks [0].method (tasks[0].unit)) {
				tasks.RemoveAt (0);
			}
		}

	

		
	}



	
	
	
	public GameObject getClosestTownCenter(GameObject c)
	{ 
		
		float aux;
		List<GameObject> towncentresX;
		if (c.tag == "Ally") towncentresX = GameController.Instance.getAllAllyTownCentres();
		else if (c.tag == "Enemy") towncentresX = GameController.Instance.getAllEnemyTownCentres();
		else return null;
		
		
		float minDistance = 100000;
		GameObject closestTown = null;
		
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
				contructingBarrack = false;
				if(GameController.Instance.OnCreate(b.GetComponent<Identity>(), UnitType.Soldier)) return true;
			}
		}
		if (! isCPUBuilding (UnitType.Barracs) & !contructingBarrack) {
			tasks.Insert (0, new Task (new Method (createBuilding), UnitType.Barracs));
			contructingBarrack = true;
		}
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
			tasks.Insert(0, new Task(new Method(createBuilding), UnitType.Archery));
     	return false;
     }

	private bool createBuilding(UnitType u)
	{
		GameObject civil = getIdleCivilian ();
		if (civil != null)
		{
			Vector3 v = new Vector3(UnityEngine.Random.Range(-20,20), 0, UnityEngine.Random.Range(-20,20));
			
			Vector3 position = civil.transform.position + v;
			if(GameController.Instance.getAllEnemyTownCentres().Count > 0)
				position = 	GameController.Instance.getAllEnemyTownCentres()[0].transform.position + v;			 
			Vector3 constructionPoint = GameController.Instance.buildingPossible(position.x,position.z,Player.CPU1,u);
			if(constructionPoint.magnitude!=0){
				GameController.Instance.createBuilding(DataManager.Instance.civilizationDatas[GameController.Instance.getAllEnemyTownCentres()[0].GetComponent<Identity>().civilization].units[u], position, new Troop( new List<GameObject>(){civil}));
				return true;
			}
				


		}
		return false;
		
	}


	public GameObject getIdleCivilian(){
	
		foreach (GameObject o in GameController.Instance.getAllEnemyCivilians()) {
			Construct c = o.GetComponent<Construct> (); 
			if (c != null) {
				if (! c.getInConstruction () & !c.getConstruct ()) {
					
						return o;
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
		if (allyAtacking.Count != 0) {
			List<GameObject> enemiesNoAtacking = getEnemiesNoAtacking (allyAtacking.Count + 1);

			for (int i = 0; i < Math.Min (allyAtacking.Count + 1, enemiesNoAtacking.Count); i++) {
				//foreach (GameObject o in enemiesNoAtacking) {
				GameObject o = enemiesNoAtacking [i];
				
				AttackController a = o.GetComponent<AttackController> ();
				if (a != null) {
					if (i < allyAtacking.Count) {
						a.attack (allyAtacking [i]);
					} else {
						a.attack (allyAtacking [0]);
					}
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
		/*if (enemiesNotBusy.Count < numTargets) {

			foreach (GameObject o in GameController.Instance.getAllEnemyCivilians()) {
				noAttacking = true;
				noConstructing = true;
				
				AttackController a = o.GetComponent<AttackController> ();
				if (a != null) {
					if(a.attacking_enemy != null){
						noAttacking = false;
						
					}
				}
				
				Construct c = o.GetComponent<Construct> (); 
				if(c != null){
					if(c.getInConstruction() || c.getConstruct()){
						
						noConstructing = false;
					}
				}
			
				if(noAttacking && noConstructing){
					enemiesNotBusy.Add(o);
				}
				
			}
		}*/
		
		return enemiesNotBusy;
	}


	



	public bool counterAttackWonder(){

		GameObject wonder = isPlayerBuildingWonder ();
	
		if (wonder != null) {
			List<GameObject> le = GameController.Instance.getAllEnemyArmy ();
			foreach(GameObject o in le) {
				AttackController a = o.GetComponent<AttackController> ();
				if (a != null) {
					a.attack (wonder);
				}
			}
			
		} else {
			return false;
		}
		return true;
		
	}

	public bool someArmyAttackingWonder(){

		foreach(GameObject o in GameController.Instance.getAllEnemyArmy ()) {
			AttackController a = o.GetComponent<AttackController> ();
			if (a != null) {
				if(a.attacking_enemy != null){
					if(a.attacking_enemy.GetComponent<Identity>().unitType == UnitType.Wonder)
						return true;
				}
				
			}
		}
		return false;
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
			tasks.Insert(0, new Task(new Method(createBuilding), UnitType.TownCenter));
		
	
	}


	public bool counterAttackMapControl(){
		foreach(Objective o in GameController.Instance.objectives){
			if (o.Controller != Player.Player) //TODO canviar quan hi hagi més d'una CPU
                return false;
		}

		if (GameController.Instance.objectives.Count > 0) {
			Objective objective = GameController.Instance.objectives [0];
			GameObject target = Instantiate(targetPrefab, objective.transform.position, Quaternion.identity) as GameObject;
            target.transform.SetParent(GameController.Instance.targetsParent.transform);
			timerDeath tD = target.GetComponent<timerDeath>();
			foreach (GameObject o in GameController.Instance.getAllEnemyArmy()){//getEnemiesNoAtacking(GameController.Instance.getAllEnemyArmy().Count + GameController.Instance.getAllEnemyCivilians().Count)) {
				tD.AddUnit(o);
				o.GetComponentInParent<UnitMovement> ().startMoving (target);
			}
		}
		return true;

		//miro quin objectiu està més aprop?? Mes aprop de que??
		       
	}

	public class Task
	{
		public Method method;
		public UnitType unit;
		public Task(Method m, UnitType u){
			method = m;
			unit = u;
		}
		
		
	}
	
	
}

