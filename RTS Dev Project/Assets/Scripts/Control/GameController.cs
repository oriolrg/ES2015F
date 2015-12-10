using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject unitsParent;
    [SerializeField] private GameObject buildingsParent;
    [SerializeField] private GameObject objectivesParent;
	public GameObject healthBarsParent;
    public GameObject targetsParent;

    [SerializeField]
	private Troop selectedUnits;

    //Keep track of all allied/enemy units, add and remove with addUnit and removeUnit.
    private List<GameObject> allAllyUnits;
    private List<GameObject> allEnemyUnits;

    private List<GameObject> allAllyArmy;
    private List<GameObject> allAllyBuildings;
	private List<GameObject> allAllyTownCentres;
    private List<GameObject> allAllyCivilians;

    private List<GameObject> allEnemyArmy;
    private List<GameObject> allEnemyBuildings;
	private List<GameObject> allEnemyTownCentres;
    private List<GameObject> allEnemyCivilians;

    private List<Troop> troops;

    //Keeps track of the resources the player has.
    [SerializeField]
    private ResourceValueDictionary playerResources;

    [SerializeField]
    private ResourceValueDictionary cpuResources;

    [SerializeField]
    private GameObject selectedPrefab;
    [SerializeField]
    private GameObject teamCirclePrefab;
    [SerializeField]
	public GameObject targetPrefab;

    public float UIheight;

    public HUD hud;

    [SerializeField] private GameObject objectivePrefab;

	private bool isSelecting;
	private Vector3 mPos;

    public List<Objective> objectives;
 	public bool placing;
 	
    //Fog of war button
    private bool fogOfWarEnabled;

    private bool justDisabled;

    private int fowCounter;
    
    // Static singleton property
    public static GameController Instance { get; private set; }


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

        troops = new List<Troop>();
        allAllyUnits = new List<GameObject>();
        allEnemyUnits = new List<GameObject>();
        allAllyArmy = new List<GameObject>();
        allAllyBuildings = new List<GameObject>();
		allAllyTownCentres = new List<GameObject>();
        allAllyCivilians = new List<GameObject>();
        allEnemyArmy = new List<GameObject>();
        allEnemyBuildings = new List<GameObject>();
		allEnemyTownCentres = new List<GameObject>();
        allEnemyCivilians = new List<GameObject>();
        selectedUnits = new Troop();
        selectedUnits.units = new List<GameObject>();

        for (int i = 0; i<10; i++)
        {
            troops.Add(new Troop());
        }

		GameStatistics.resetStatistics();
    }

    // Use this for initialization
    void Start ()
    {
        selectedUnits = new Troop();
        initResourceValues();
        if (!GameData.sceneFromMenu)
        {
            spawnRandomObjectives();
        }
        placing = false;
        fogOfWarEnabled = true; //fog of war enabler and disabler
        justDisabled = false; //fog of war enabler and disabler
        fowCounter = 0;
    }

	IEnumerator ToGameStatisticsIEnumerator(Player winner, Victory winCondition){
		GameStatistics.winner = winner;
		GameStatistics.winCondition = winCondition;
		yield return new WaitForSeconds(3);
		Application.LoadLevel("EndGameScene");
	}

	public void ToGameStatistics(Vector3 poi, Player winner, Victory winCondition){
		Vector3 newCameraPosition;
		
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (
			ray, out hit,
			Mathf.Infinity // max distance
			)
	    ) {
			newCameraPosition = poi + (Camera.main.transform.position - hit.point);
		} else {
			throw new UnityException("POI isn't over ground!");
		}
		
		Camera.main.transform.position = newCameraPosition;
		
		Time.timeScale = 0; // pause the game

		StartCoroutine(ToGameStatisticsIEnumerator(winner, winCondition));
	}

    // Update is called once per frame
    void Update()
    {
		// TODO: Trial code for Alvaro. Erase when bug with EndGameMenu is solved
		// hud.gameMenu.GetComponent<GameMenuBehaviour>().EndGameMenu(Vector3.zero, true, "hola");

        if (Input.mousePosition.y > Screen.height * UIheight)
        {


            //Left Click Manager
            if (Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
                mPos = Input.mousePosition;

                //Click detection
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {

                    GameObject selectedGO = hitInfo.transform.gameObject;
                    if (hitInfo.transform.gameObject.tag == "Ally")
                    {

                        if (!Input.GetKey(KeyCode.LeftControl)) ClearSelection();

                        if (!selectedUnits.units.Contains(selectedGO)) selectedUnits.units.Add(selectedGO);

                        selectedUnits.FocusedUnit = selectedGO;
                        Transform projector = selectedGO.transform.FindChild("Selected");
                        if (projector != null)
                            projector.gameObject.SetActive(true);
                        hud.updateSelection(selectedUnits);
                        

                    }
                    else
                    {
                        // Debug.Log("not Ally");
                    }
                }
                else
                {
                    // Debug.Log("No hit");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                
                //Click detection
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                GameObject target;
                if (hit)
                {
                    
                    if (AI.Instance.resources.Contains(hitInfo.transform.gameObject.tag))
                        moveUnits(hitInfo.transform.gameObject);
                    else if (hitInfo.transform.gameObject.tag == "Enemy")
                    {
                        GameObject enemy = hitInfo.transform.gameObject;
                        attack(enemy);
                    }

                    else if (hitInfo.transform.gameObject.tag == "Ally" && hitInfo.transform.gameObject.GetComponent<BuildingConstruction>().getConstructionOnGoing())
                    {
                        noAttack();
                        Debug.Log("vaig a construir");

                        Troop troop = new Troop(selectedUnits.units);

                        foreach (var unit in troop.units)
                        {
                            Construct scriptConstruct = unit.GetComponent<Construct>();

                            if (scriptConstruct != null)
                            {

                                if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                                {
                                    scriptConstruct.setConstruct(false);
                                    scriptConstruct.SetInConstruction(false);
                                    scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                                }

                                scriptConstruct.SetBuildingToConstruct(hitInfo.transform.gameObject);
                            }
                        }

                        buildingConstruction(hitInfo.transform.gameObject.transform.position, troop);

                    }
                    else
                    {
                        
                        Identity identity = hitInfo.transform.GetComponent<Identity>();
                        if (identity != null && identity.unitType.isBuilding())
                        {
                            
                            // We hit a building
                            moveUnits(identity.gameObject);
                        }
                        else
                        {
                            
                            // We hit the ground
                            noAttack();
                            target = Instantiate(targetPrefab, hitInfo.point, Quaternion.identity) as GameObject;
                            target.transform.SetParent(targetsParent.transform);
                            moveUnits(target);
                        }
                    }
                }
                else
                {
                    // Debug.Log("No hit");
                }
            }
        }

        //End of click
        if (Input.GetMouseButtonUp(0))
        {

            if (isSelecting)
            {
                isSelecting = false;

                //We impose a size of 5 to detect a box.
                //Box Selection
                Vector3 maxVector = new Vector3(Input.mousePosition.x, Mathf.Max(Input.mousePosition.y, UIheight * Screen.height), Input.mousePosition.z);
                if ((mPos - maxVector).magnitude > 5)
                {
                    var camera = Camera.main;
                    var viewportBounds = RectDrawer.GetViewportBounds(camera, mPos, maxVector);

                    //Deselecting
                    if (!Input.GetKey(KeyCode.LeftControl)) ClearSelection();

                    //Selecting
                    foreach (var unit in FindObjectsOfType<GameObject>())
                    {
                        //Units inside the rect get selected.
                        if (viewportBounds.Contains(camera.WorldToViewportPoint(unit.transform.position)) & unit.tag == "Ally" & !selectedUnits.units.Contains(unit))
                        {
                            selectedUnits.units.Add(unit);
                            Transform projector = unit.transform.FindChild("Selected");
                            if (projector != null)
                                projector.gameObject.SetActive(true);
                        }

                    }
                    if (selectedUnits.units.Count > 0) selectedUnits.FocusedUnit = selectedUnits.units[0];
                    hud.updateSelection(selectedUnits);
                }
            }

        }

        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKey("" + i))
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    troops[i].units.Clear();
                    foreach (var unit in selectedUnits.units)
                    {
                        troops[i].units.Add(unit);
                    }
                    print(troops[i].units[0]);
                }
                else
                {
                    if (troops[i].units.Count > 0)
                    {
                        ClearSelection();
                        foreach (var unit in troops[i].units)
                        {
                            selectedUnits.units.Add(unit);
                            Transform projector = unit.transform.FindChild("Selected");
                            if (projector != null)
                                projector.gameObject.SetActive(true);
                        }
                        if (selectedUnits.units.Count > 0) selectedUnits.FocusedUnit = selectedUnits.units[0];
                        hud.updateSelection(selectedUnits);
                    } 
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            selectedUnits.focusNext();
            hud.updateSelection(selectedUnits); // There will exist an updateFocus method            
        }

		if (Input.GetKeyDown(KeyCode.B))
		{
            //createCubeTestingGrid();
		}
        if(Input.GetKeyDown(KeyCode.K))
        {
            if( selectedUnits.FocusedUnit != null )
                selectedUnits.FocusedUnit.GetComponent<AttackController>().attack(selectedUnits.FocusedUnit);
        }
        if (Input.GetKeyDown(KeyCode.F) || justDisabled)
        {
            //Fog of war button
            if (fogOfWarEnabled)
            {
                print("FOG OF WAR BUTTON PRESSED");
                GameObject.FindGameObjectsWithTag("Ally")[0].GetComponent<LOSEntity>().Range = 1000;
                fogOfWarEnabled = false;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(250, 20, 250);
                cube.tag = "FOWObject";
                
                cube.AddComponent<LOSEntity>();
                cube.GetComponent<LOSEntity>().Range = 1000;
                cube.GetComponent<MeshRenderer>().enabled = false;

                justDisabled = true;
                
            }
            else
            {
                
                fowCounter++;
                //foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
                //{
                //    g.GetComponent<LOSEntity>().reset();
                //}
                //GameObject.FindGameObjectWithTag("Ground").GetComponent<LOSManager>().forceFullUpdate();
                if (fowCounter > 5)
                {
                    GameObject.FindGameObjectWithTag("FOWObject").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.FindGameObjectWithTag("Ground").GetComponent<LOSManager>().enabled = false;
                    justDisabled = false;
                    fowCounter = 0;
                    foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        g.GetComponent<LOSEntity>().reset();
                    }

                }
            }

        }

    }




	void OnGUI()
	{
		//Drawing of the box while selecting.
		if( isSelecting )
		{
            Vector3 maxVector = new Vector3(Input.mousePosition.x, Mathf.Max(Input.mousePosition.y, UIheight * Screen.height), Input.mousePosition.z);
            var rect = RectDrawer.GetScreenRect(mPos, maxVector );
			RectDrawer.DrawScreenRect( rect, new Color( 0.6f, 0.9f, 0.6f, 0.25f ) );
            RectDrawer.DrawScreenRectBorder(rect, 3.0f, new Color(0.6f, 1.0f, 0.6f, 0.33f));
        }
	}

    public void spawnRandomObjectives()
    {
        objectives = new List<Objective>();

        int ammount = UnityEngine.Random.Range(2, 5);
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        Bounds bounds = ground.GetComponent<TerrainCollider>().bounds;

        for ( int i = 0; i < ammount; i++ )
        {
            Vector3 position = new Vector3
            (
                bounds.center.x + UnityEngine.Random.Range(-bounds.extents.x / 2, bounds.extents.x / 2),
                500,
                bounds.center.z + UnityEngine.Random.Range(-bounds.extents.z / 2, bounds.extents.z / 2)
            );

            // Adjust to terrain hit
            Ray ray = new Ray(position, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject go = Instantiate(objectivePrefab, hitInfo.point, Quaternion.identity) as GameObject;

                LOSEntity script = go.GetComponent<LOSEntity>();
                script.enabled = false;
                script.enabled = true;

                objectives.Add(go.GetComponentOrEnd<Objective>());
                go.transform.SetParent(objectivesParent.transform);
            }
        }
    }

	public void moveUnits(GameObject target)
	{
        if (selectedUnits.units.Count == 0) return;
        timerDeath groundTarget = target.GetComponent<timerDeath>();
        int formationMatrixSize = (int)Math.Ceiling(Math.Sqrt(selectedUnits.units.Count)); 
        if (selectedUnits.hasMovableUnits())
        {
			if(groundTarget != null)
           		groundTarget.setFormationMatrix(formationMatrixSize);
            foreach (var unit in selectedUnits.units)
            {
                // Special case: a civilian moves towards a town center and has resources to store
                Identity identity = unit.GetComponent<Identity>();
                Identity targetIdentity = target.GetComponent<Identity>();
                CollectResources collect = unit.GetComponent<CollectResources>();

                if ( identity != null && identity.unitType == UnitType.Civilian && targetIdentity != null && targetIdentity.unitType == UnitType.TownCenter && collect != null && collect.totalCollected() > 0)
                {
                    unit.GetComponent<UnitMovement>().startMoving(target, unit.GetComponent<CollectResources>().store);
                }
                else
                {

                    Construct scriptConstruct = unit.GetComponent<Construct>();

                    if (scriptConstruct != null)
                    {

                        if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                        {
                            scriptConstruct.setConstruct(false);
                            scriptConstruct.SetInConstruction(false);
                            scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                        }
                    }

                    UnitMovement script = unit.GetComponentInParent<UnitMovement>();
                    if (script != null)
                    {
                        collect = unit.GetComponent<CollectResources>();
                        if (collect != null && AI.Instance.resources.Contains(target.tag))
                        {

                            collect.targetObject = target;
                            collect.startMovingToCollect(collect.targetObject);
                        }
                        else
                        {
                            if(formationMatrixSize > 1){
                                Vector3 newTargetPosition = Vector3.zero;
                                if( groundTarget != null )
                                    newTargetPosition = groundTarget.AddUnitMouseSelection(unit);
                                
                                script.startMoving(target);
                                script.targetPos = newTargetPosition;
                            } else {
								if(groundTarget != null)
									groundTarget.AddUnit(unit);
								
                                script.startMoving(target);
                            }
                        }
                    }
                }
            }
            
        }
        else
        {
            foreach (var unit in selectedUnits.units)
            {
                Spawner script = unit.GetComponentInParent<Spawner>();
                if (script != null)
                {
                    script.RallyPoint = target.transform.position;
                    script.customRally = true;
                }
            }
            Destroy(target.gameObject);
        }
	}

    private void moveUnits(GameObject target, Troop troop)
    {
        if (troop.hasMovableUnits())
        {
            foreach (var unit in troop.units)
            {

                Construct scriptConstruct = unit.GetComponent<Construct>();

                if (scriptConstruct != null)
                {

                    if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                    {
                        scriptConstruct.setConstruct(false);
                        scriptConstruct.SetInConstruction(false);
                        scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                    }

                }

                UnitMovement script = unit.GetComponentInParent<UnitMovement>();
                if (script != null)
                {
                    script.startMoving(target);
                    target.GetComponent<timerDeath>().AddUnit(unit);
                }

            }
        }
        else
        {
            foreach (var unit in troop.units)
            {
                Spawner script = unit.GetComponentInParent<Spawner>();
                if (script != null)
                {
                    script.RallyPoint = target.transform.position;
                }
            }
            Destroy(target.gameObject);
        }
    }

    public void addUnit(GameObject u)
    {
        if (u.tag == "Ally")
        {
            if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Civilian)
            {
                allAllyCivilians.Add(u);
            } else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
                || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
                || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
                || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                allAllyBuildings.Add(u);
				if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter) allAllyTownCentres.Add (u);
            } else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
                || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
                || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                allAllyArmy.Add(u);
            }
        }
        if (u.tag == "Enemy")
        {
            if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Civilian)
            {
                allEnemyCivilians.Add(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                allEnemyBuildings.Add(u);
				if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter) allEnemyTownCentres.Add (u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                allEnemyArmy.Add(u);
            }
        }
    }

    public void removeUnit(GameObject u)
    {
        if (u.tag == "Ally")
        {
            if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Civilian)
            {
                GameStatistics.addKilledUnits(Player.CPU1, 1);
                GameStatistics.addLostUnits(Player.Player, 1);
                allAllyCivilians.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                GameStatistics.addKilledUnits(Player.CPU1, 1);
                GameStatistics.addLostUnits(Player.Player, 1); //Buildings
                allAllyBuildings.Remove(u);
				if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter) allAllyTownCentres.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                GameStatistics.addKilledUnits(Player.CPU1, 1);
                GameStatistics.addLostUnits(Player.Player, 1);
                allAllyArmy.Remove(u);
            }
        }
        if (u.tag == "Enemy")
        {
            if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Civilian)
            {
                GameStatistics.addKilledUnits(Player.Player, 1);
                GameStatistics.addLostUnits(Player.CPU1, 1);
                allEnemyCivilians.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                allEnemyBuildings.Remove(u);
                GameStatistics.addKilledUnits(Player.Player, 1);
                GameStatistics.addLostUnits(Player.CPU1, 1);
                if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter) allEnemyTownCentres.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                GameStatistics.addKilledUnits(Player.Player, 1);
                GameStatistics.addLostUnits(Player.CPU1, 1);
                allEnemyArmy.Remove(u);
            }
        }

		if (GameData.winConditions.Contains (Victory.Annihilation)){
			checkBuildingDestruction(u);
		}

        if (selectedUnits.units.Contains(u))
        {
            Transform projector = u.transform.FindChild("Selected");
            if (projector != null) projector.gameObject.SetActive(false);
            selectedUnits.units.Remove(u);
        }

        if(selectedUnits.FocusedUnit == u)
        {
            hud.ClearSelection();
        }
    }

    //Set starting resource values.
    public void initResourceValues()
    {
        /*playerResources[Resource.Food] = 1000;
        playerResources[Resource.Wood] = 1000;
        playerResources[Resource.Metal] = 1000;
        playerResources[Resource.Population] = 100;
        cpuResources[Resource.Food] = 1000;
        cpuResources[Resource.Wood] = 1000;
        cpuResources[Resource.Metal] = 1000;
        cpuResources[Resource.Population] = 100;
        hud.updateResource(Resource.Food, playerResources[Resource.Food]);
        hud.updateResource(Resource.Wood, playerResources[Resource.Wood]);
        hud.updateResource(Resource.Metal, playerResources[Resource.Metal]);
        hud.updateResource(Resource.Population, playerResources[Resource.Population]);*/
		playerResources[Resource.Food] = 100000;
		playerResources[Resource.Wood] = 100000;
		playerResources[Resource.Metal] = 100000;
		playerResources[Resource.Population] = 10000;
		cpuResources[Resource.Food] = 1000;
		cpuResources[Resource.Wood] = 1000;
		cpuResources[Resource.Metal] = 1000;
		cpuResources[Resource.Population] = 100;
		hud.updateResource(Resource.Food, playerResources[Resource.Food]);
		hud.updateResource(Resource.Wood, playerResources[Resource.Wood]);
		hud.updateResource(Resource.Metal, playerResources[Resource.Metal]);
		hud.updateResource(Resource.Population, playerResources[Resource.Population]);
    }

    //Called to check whether there are enough resources to perform an action.
    //Params: resource type, amount (to subtract).
    //Returns true if there are enough, and updates the resource type.
    //Returns false if there aren't enough, and displays warnings.
    public bool checkResources(ResourceValueDictionary resourceCosts, String player)
    {
        ResourceValueDictionary resDict;
        if (player == "Ally") resDict = playerResources;
        else resDict = cpuResources;
        bool check = true;
        foreach (KeyValuePair<Resource, int> kv in resourceCosts)
        {
            if (resDict[kv.Key] - kv.Value < 0)
            {
                //Here goes stuff that happens when there aren't enough resources to perform the action.
                //i.e. text pop-up, sound warning.
                check = false;
            }
        }
        return check;
    }

    public void updateResource(ResourceValueDictionary resourceCosts, String player)
    {
        foreach (KeyValuePair<Resource, int> kv in resourceCosts)
        {
            updateResource(kv.Key, kv.Value, player);
        }
    }

    public void updateResource(Resource res, int value, String player)
    {
        ResourceValueDictionary resDict;
        if (player == "Ally") resDict = playerResources;
        else resDict = cpuResources;
        resDict[res] -= value;
        if (player=="Ally")hud.updateResource(res, resDict[res]-value); //Subtracting the value twice fixes update on resource panel as one times the cost is given back after OnActionButtonExit.
    }

    public void updateResource(Resource res, int value)
    {
        playerResources[res] -= value;
        hud.updateResource(res, playerResources[res]); 
		print ("alone");
    }

    public void updateInteractable()
    {
        hud.updateInteractable(selectedUnits.FocusedUnit);
    }

    public void updateControl( GameObject go )
    {
        if (go == selectedUnits.FocusedUnit)
            hud.updateControl(go);
    }

    public void updateHealth(GameObject go )
    {
        if (go == selectedUnits.FocusedUnit)
            hud.updateHealth(go);
    }

    public void updateRightPanel( GameObject go )
    {
    }

    public void checkMapControl(GameObject lastObjectControlled)
    {
        hud.updateSelection(selectedUnits);

		if (!GameData.winConditions.Contains (Victory.MapControl))
			return; // don't check it; it's not a win condition

		if (objectives.Count == 0)
			return; // nothing to check

        Player possibleWinner = objectives[0].Controller;

        foreach(Objective objective in objectives)
        {
            if (objective.Controller != possibleWinner)
                return;
        }

        if (possibleWinner != Player.Neutral)
        {
            hud.startCountdown(lastObjectControlled.transform.position, Victory.MapControl, possibleWinner);
            InvokeRepeating("ensureWinner", 1, 1);
        }
    }

    public void ensureWinner()
	{
		if (!GameData.winConditions.Contains (Victory.MapControl))
			return; // don't check it; it's not a win condition

		if (objectives.Count == 0)
			return; // nothing to check here

        Player possibleWinner = objectives[0].Controller;
        foreach (Objective objective in objectives)
        {
            if (objective.Controller != possibleWinner)
            {
                hud.stopCountdown(Victory.MapControl);
                return;
            }
        }
    }

    public void checkBuildingDestruction(GameObject lastBuilding)
    {
		if (!GameData.winConditions.Contains(Victory.Annihilation))
			return; // not a win condition

		if (allEnemyBuildings.Count == 0){ // TODO: Correct this for multiple CPUs
			GameController.Instance.ToGameStatistics(lastBuilding.transform.position, Player.Player, Victory.Annihilation);
			/*hud.gameMenu.GetComponent<GameMenuBehaviour>().EndGameMenu(
				lastBuilding.transform.position, true, "You destroyed all enemy buildings"
			);*/
		} else if(allAllyBuildings.Count == 0) {
			GameController.Instance.ToGameStatistics(lastBuilding.transform.position, Player.CPU1, Victory.Annihilation);
			/*hud.gameMenu.GetComponent<GameMenuBehaviour>().EndGameMenu(
				lastBuilding.transform.position, false, "All your buildings were destroyed"
			);*/
		}
    }

    public void reloadLevel()
    {
        Application.Quit();
    }

    // Called when selected units are destroyed
    public void ClearSelection()
    {
        if (selectedUnits.units.Count > 0)
        { 
            foreach (var unit in selectedUnits.units)
            {
                Transform projector = unit.transform.FindChild("Selected");
                if (projector != null)
                    projector.gameObject.SetActive(false);
            }
            selectedUnits.units.Clear();
            selectedUnits.FocusedUnit = null;
            hud.ClearSelection();
        }
    }

    public GameObject CreateUnit(GameObject building, GameObject unit)
    {

        Spawner spawner = building.GetComponentOrEnd<Spawner>();

        spawner.initBounds();
        Vector3 spawningPoint = spawner.SpawningPoint;
        Vector3 rallyPoint = spawner.RallyPoint;
        
        Ray ray = new Ray(rallyPoint + new Vector3(0, 100, 0), -Vector3.up);

        bool freeSpaceFound = false;

        RaycastHit hitInfo = new RaycastHit();

        int multiplier = 2;

        while (!freeSpaceFound && Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.transform.tag == "Ground")
            {
                //Noone there

                bool someoneElse = false;
                foreach (Transform target in targetsParent.transform)
                {
                    if (target.position == hitInfo.point)
                        someoneElse = true;
                }

                if (someoneElse)
                {
                    ray.origin += Vector3.right * multiplier;
                    multiplier = (int)-Mathf.Sign(multiplier) * (Mathf.Abs(multiplier) + 2);

                }
                else
                {
                    freeSpaceFound = true;
                }
            }
            else
            {
                ray.origin += Vector3.right * multiplier;
                multiplier = (int)-Mathf.Sign(multiplier) * (Mathf.Abs(multiplier) + 2);
            }

        }

        if (freeSpaceFound)
        {


            GameObject newUnit = Instantiate(unit, spawner.SpawningPoint, Quaternion.identity) as GameObject;


            // Set unit as parent in hierarchy
            newUnit.transform.SetParent(unitsParent.transform);
            GameObject target = Instantiate(targetPrefab, spawner.RallyPoint, Quaternion.identity) as GameObject;
            
            target.transform.SetParent(targetsParent.transform);
            UnitMovement script = newUnit.GetComponent<UnitMovement>();
            if (script != null)
            {
                script.startMoving(target);
                target.GetComponent<timerDeath>().AddUnit(newUnit);
            }
            return newUnit;
        }
        else
        {
            return null;
        }
    }
    

    public GameObject createBuilding(GameObject prefab)
	{
        //Instantiate the building and start the positioning of the building
        GameObject building = Instantiate (prefab, Vector3.zero, gameObject.transform.rotation) as GameObject;

        building.transform.SetParent(buildingsParent.transform);

        building.tag = selectedUnits.units[0].gameObject.tag;

        Identity newIden = building.GetComponent<Identity>();
        if (newIden != null) newIden.player = selectedUnits.units[0].GetComponent<Identity>().player;

        addSelectedPrefab(building);
        addTeamCirclePrefab(building);

        building.GetComponent<BuildingConstruction>().setConstructionOnGoing(true);
        
        updateInteractable();

        foreach (var unit in selectedUnits.units)
        {
            Construct scriptConstruct = unit.GetComponent<Construct>();

            if (scriptConstruct != null)
            {

                if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                {
                    scriptConstruct.setConstruct(false);
                    scriptConstruct.SetInConstruction(false);
                    scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                }
                scriptConstruct.SetBuildingToConstruct(building);


            }
        }
        
        building.AddComponent<BuildingPlacer> ();

        placing = true;

        enabled = false;
        
        
        //createBuilding(prefab, new Vector3(52.8f, 0, 42.7f),selectedUnits);   
        return building;  

        
	}


    public void createBuilding(GameObject prefab, Vector3 position, Troop t)
    {
        GameObject building = Instantiate(prefab, position, gameObject.transform.rotation) as GameObject;

        building.transform.SetParent(buildingsParent.transform);

        building.tag = t.units[0].gameObject.tag;

        Identity newIden = building.GetComponent<Identity>();
        if (newIden != null) newIden.player = t.units[0].GetComponent<Identity>().player;

	LOSEntity fow = building.GetComponent<LOSEntity>();
        if (fow != null) fow.IsRevealer = false;

        addSelectedPrefab(building);
        addTeamCirclePrefab(building);

        BuildingConstruction build = building.GetComponent<BuildingConstruction>();

        if (build != null)
        {
            build.setFinalMesh();

            building.GetComponent<MeshFilter>().mesh = build.getInitialMesh().GetComponent<MeshFilter>().sharedMesh;

            build.setConstructionOnGoing(true);

        }


        //building.GetComponent<BuildingConstruction>().setConstructionOnGoing(true);

        //foreach (var unit in t.units) unit.GetComponent<Construct>().SetBuildingToConstruct(building);

        foreach (var unit in t.units)
        {
            Construct scriptConstruct = unit.GetComponent<Construct>();

            if (scriptConstruct != null)
            {

                if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                {
                    scriptConstruct.setConstruct(false);
                    scriptConstruct.SetInConstruction(false);
                    scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                }
                scriptConstruct.SetBuildingToConstruct(building);


            }
        }

        buildingConstruction(position, t);

    }

    public void buildingConstruction(Vector3 position, Troop t)
    {
        placing = false;
        //Debug.Log("tag dintre building construction" + t.units[0].GetComponent<Construct>().getBuildingToConstruct().tag);

        
        //LOSEntity fow = t.units[0].GetComponent<Construct>().getBuildingToConstruct().GetComponent<LOSEntity>();
        //if (fow != null) fow.IsRevealer = false;

        foreach (var unit in t.units)
        {
            if (unit.tag != unit.GetComponent<Construct>().getBuildingToConstruct().tag) t.units.Remove(unit);
        }

        //Move the units that are selected to construct to the building position
        GameObject target = Instantiate(targetPrefab, position, Quaternion.identity) as GameObject;
        target.transform.SetParent(targetsParent.transform);
        moveUnits(target, t);

        //Order that the unit has to construct
        foreach (var unit in t.units) unit.GetComponent<Construct>().setConstruct(true);

        enabled = true;//Enable the script 
    }
	public Vector3 buildingPossible(float x, float z,Player p, UnitType u){
		Vector3 point = new Vector3 (0, 0, 0);
		Ray ray = new Ray (new Vector3 (x, 1000, z), -Vector3.up);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		
		bool groundHitFound = false;
		RaycastHit groundHit = default(RaycastHit);		
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.gameObject.tag == "Ground")
			{

				Collider[] hitColliders = Physics.OverlapSphere(hit.point, DataManager.Instance.civilizationDatas[GameData.playerToCiv(p)].units[u].GetComponent<Renderer>().bounds.extents.magnitude);
				print (DataManager.Instance.civilizationDatas[GameData.playerToCiv(p)].units[u].GetComponent<Renderer>().bounds.extents.magnitude);
				foreach(Collider c in hitColliders){
					print (c.gameObject.name);
					if(c.tag!="Ground"&c.gameObject.name!=gameObject.name){
						return point;
					}
				}
				return hit.point;

			}
		}
		return point;
	}

    // Action events
    public void OnActionButtonEnter(String description, ResourceValueDictionary resourceCost)
    {
        hud.OnActionButtonEnter(description, resourceCost);
    }

    public void OnActionButtonExit(String description, ResourceValueDictionary resourceCost)
    {
        hud.OnActionButtonExit(description, resourceCost);
    }
    
    public void hideRightPanel()
    {
        hud.hideRightPanel();
    }

    public Troop getSelectedUnits()
    {
        return selectedUnits;
    }

    public void moveSelection()
    {
        print("move");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }

    public void stopSelection()
    {
        print("stop");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }

    public bool OnCreate( Identity who, UnitType what )
    {
		bool done = false;

        if (!placing)
        {
            // get the unit data and the prefab of the unit that can be created
            GameObject prefab = DataManager.Instance.civilizationDatas[who.civilization].units[what];
            UnitData unitData = DataManager.Instance.unitDatas[what];
            //print("Creating " + unitData.description);

        if (what.isBuilding())
        {
            if (checkResources(unitData.resourceCost, who.tag))
            {
                //Create the building
                GameObject created = createBuilding(prefab);
                created.tag = who.gameObject.tag;

                Identity newIden = created.GetComponent<Identity>();
                if (newIden != null) newIden.player = who.player;
                GameStatistics.addCreatedUnits(who.player, 1); //buidings

                if (who.tag == "Ally") addSelectedPrefab(created);
                addTeamCirclePrefab(created);

                Spawner spa = created.GetComponent<Spawner>();
                if (spa != null) spa.initBounds();
                //updateResource(unitData.resourceCost, who.tag);
            }
        }
        else
        {
            Action action = new Action(unitData.preview, unitData.requiredTime, () =>
            {
                GameObject created = CreateUnit(who.gameObject, prefab);
				created.tag = who.gameObject.tag;

                Identity newIden = created.GetComponent<Identity>();
                if (newIden != null) newIden.player = who.player;
                GameStatistics.addCreatedUnits(who.player, 1);

                if (who.tag == "Ally")
                {
                    addSelectedPrefab(created);
                    hud.updateDelayedActions(selectedUnits.FocusedUnit);
                }
                addTeamCirclePrefab(created);
            });

                //create an action and add it to the focused unit's queue
                if (who.gameObject.GetComponentOrEnd<DelayedActionQueue>().Enqueue(action))
                {
                    if (checkResources(unitData.resourceCost, who.tag))
                    {
                        //DelayedActionQueue script = who.gameObject.GetComponentOrEnd<DelayedActionQueue>();
                        //script.Enqueue(action);
                        updateResource(unitData.resourceCost, who.tag);
                        if (who.gameObject.tag == "Ally") hud.updateDelayedActions(selectedUnits.FocusedUnit);
                        done = true;
                    }
                }
            }
        }

        return done;

     }

    public void addSelectedPrefab(GameObject go)
    {
        if (go.transform.FindChild("Selected") == null)
        {
            GameObject selectedProj = Instantiate(selectedPrefab, go.transform.position + new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
            //selectedProj.transform.Rotate(90, 0, 0);
            selectedProj.name = "Selected";
            selectedProj.SetActive(false);
            selectedProj.transform.SetParent(go.transform);
            selectedProj.transform.up = new Vector3(0, 0, 1);
            SelectionCircle script = selectedProj.GetComponent<SelectionCircle>();
            if (script != null) script.init();
        }
    }

    public void addSelectedPrefabstoCurrentUnits()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach( GameObject ally in allies )
        {
            addSelectedPrefab(ally);
        }
    }

    public void addTeamCirclePrefab(GameObject go)
    {
        if (go.transform.FindChild("TeamCircle") == null)
        {
            Identity iden = go.GetComponent<Identity>();
            GameObject teamProj = Instantiate(teamCirclePrefab, go.transform.position + new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
            //selectedProj.transform.Rotate(90, 0, 0);
            teamProj.name = "TeamCircle";
            teamProj.SetActive(true);
            teamProj.transform.SetParent(go.transform);
            teamProj.transform.up = new Vector3(0, 0, 1);
            TeamCircleProjector script = teamProj.GetComponent<TeamCircleProjector>();
            if (script != null)
            {
                if (iden != null) script.initWithTeamColor(iden);
                else script.init();
            }
        }
    }

    public void addTeamCirclePrefabstoCurrentUnits()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject ally in allies)
        {
            addTeamCirclePrefab(ally);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            addTeamCirclePrefab(enemy);
        }
    }

    public void OnSacrifice()
    {
        if (!placing)
        {
            GameObject unit = GameController.Instance.selectedUnits.FocusedUnit;
            Health health = unit.GetComponent<Health>();

            //Reset construction
            Construct scriptConstruct = unit.GetComponent<Construct>();

            if (scriptConstruct != null)
            {

                if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                {
                    scriptConstruct.setConstruct(false);
                    scriptConstruct.SetInConstruction(false);
                    scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                }
            }


            if (health != null)
            {
                GameStatistics.addKilledUnits(Player.CPU1, -1);
                health.die();
            }
            else
            {
                GameController.Instance.hud.showMessageBox("Not implemented");
            }
        }
    }

    public void OnAttack()
    {
        print("attack");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }

	public void noAttack(){
		Troop troop = new Troop(selectedUnits.units);
        if (troop.units.Count != 0)
        {
            AttackController atkController;
            foreach (var unit in troop.units)
            {
                atkController = unit.GetComponent<AttackController>();
                if (atkController != null)
                {
                    atkController.attacking_enemy = null;
                    atkController.attacking_target = null;
                }
            }
        }
	}

    public void attack(GameObject enemy)
    {
        Troop troop = new Troop(selectedUnits.units);
        if (troop.units.Count != 0)
        {
            AttackController atkController;
            foreach (var unit in troop.units)
            {
                //Reset construction
                Construct scriptConstruct = unit.GetComponent<Construct>();

                if (scriptConstruct != null)
                {

                    if (scriptConstruct.getConstruct() || scriptConstruct.getInConstruction())
                    {
                        scriptConstruct.setConstruct(false);
                        scriptConstruct.SetInConstruction(false);
                        scriptConstruct.getBuildingToConstruct().GetComponentOrEnd<BuildingConstruction>().deleteUnit(unit);
                    }
                }

                atkController = unit.GetComponent<AttackController>();
                atkController.attack(enemy);
            }
        }
    }

    public List<GameObject> getAllAllyArmy()
    {
        return allAllyArmy;
    }
    public List<GameObject> getAllAllyBuildings()
    {
        return allAllyBuildings;
    }
	public List<GameObject> getAllAllyTownCentres()
	{
		return allAllyTownCentres;
	}
    public List<GameObject> getAllAllyCivilians()
    {
        return allAllyCivilians;
    }
    public List<GameObject> getAllEnemyArmy()
    {
        return allEnemyArmy;
    }
	public List<GameObject> getAllEnemyTownCentres()
	{
		return allEnemyTownCentres;
	}

    public List<GameObject> getAllEnemyBuildings()
    {
        return allEnemyBuildings;
    }
    public List<GameObject> getAllEnemyCivilians()
    {
        return allEnemyCivilians;
    }
    public ResourceValueDictionary getPlayerResources()
    {
        return playerResources;
    }
    public ResourceValueDictionary getCPUResources()
    {
        return cpuResources;
    }

}
