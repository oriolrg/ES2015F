using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject unitsParent;
    [SerializeField] private GameObject buildingsParent;
    [SerializeField] private GameObject objectivesParent;
    [SerializeField] private GameObject targetsParent;

    [SerializeField]
	private Troop selectedUnits;

    //Keep track of all allied/enemy units, add and remove with addUnit and removeUnit.
    private List<GameObject> allAllyUnits;
    private List<GameObject> allEnemyUnits;

    private List<GameObject> allAllyArmy;
    private List<GameObject> allAllyBuildings;
    private List<GameObject> allAllyCivilians;

    private List<GameObject> allEnemyArmy;
    private List<GameObject> allEnemyBuildings;
    private List<GameObject> allEnemyCivilians;

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
	private GameObject targetPrefab;

    [SerializeField]
    private float UIheight;

    public HUD hud;

    [SerializeField] private GameObject objectivePrefab;

	private bool isSelecting;
	private Vector3 mPos;

    public List<Objective> objectives;



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

        allAllyUnits = new List<GameObject>();
        allEnemyUnits = new List<GameObject>();
        allAllyArmy = new List<GameObject>();
        allAllyBuildings = new List<GameObject>();
        allAllyCivilians = new List<GameObject>();
        allEnemyArmy = new List<GameObject>();
        allEnemyBuildings = new List<GameObject>();
        allEnemyCivilians = new List<GameObject>();
        selectedUnits = new Troop();
        selectedUnits.units = new List<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        selectedUnits = new Troop();
        initResourceValues();
        addTeamCirclePrefabstoCurrentUnits();
        spawnRandomObjectives();
        addSelectedPrefabstoCurrentUnits();
    }

    // Update is called once per frame
    void Update()
    {
        

        //Win Condition
        var wonder = GameObject.FindGameObjectWithTag("Wonder");
        if (wonder != null)
        {
            winCondition();
        }

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
                    else if(hitInfo.transform.gameObject.tag == "Enemy")
                    {
						/*
						GameObject enemy = hitInfo.transform.gameObject;
						//Crear nou metode moveUnit
						//Crear interficie de atac!!!!!!!!!!!!!
						foreach(var ally in selectedUnits.units){
							ally.GetComponent<UnitMovement>();
						}


						GameObject allyUnit = selectedUnits.units[0];

						Vector3 allyPos = allyUnit.transform.position;

						double d = Vector3.Distance(allyPos,enemyPos);

						Vector3 vec =- allyPos + enemyPos;

						vec = vec.normalized;

						double r = allyUnit.GetComponent<attack_controller>().range;

						Debug.Log(r);

						double alpha =  d-(r/2.0);


						vec.x *= (float) alpha;
						vec.z *= (float) alpha;

						Vector3 targetPos = vec + allyPos;


                        target = Instantiate(targetPrefab, targetPos, Quaternion.identity) as GameObject;
                        moveUnits(target);
                        */

                    } 
		            else if(hitInfo.transform.gameObject.tag == "Ally" && hitInfo.transform.gameObject.GetComponent<BuildingConstruction>().getConstructionOnGoing())
                    {
                        Debug.Log("vaig a construir");

                        Troop troop = new Troop(selectedUnits.units);

                        foreach (var unit in troop.units)
                        {
                            Construct scriptConstruct = unit.GetComponent<Construct>();

                            if (scriptConstruct != null) {

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
			            target = Instantiate(targetPrefab, hitInfo.point, Quaternion.identity) as GameObject;
                        target.transform.SetParent(targetsParent.transform);
			            moveUnits(target);
		            }
                }
                else
                {
                    // Debug.Log("No hit");
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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                selectedUnits.focusNext();
                hud.updateSelection(selectedUnits); // There will exist an updateFocus method            
            }

			if (Input.GetKeyDown(KeyCode.B))
			{
                //Pobre Sergiot
                //createCubeTestingGrid();
			}

            if(Input.GetKeyDown(KeyCode.M))
            {
                for (int i = allAllyUnits.Count - 1; i >= 0; i--)
                {
                    print(allAllyUnits[i].name);
                    allAllyUnits[i].GetComponent<Health>().die();
                }
                hud.showMessageBox("MM");
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

    private void spawnRandomObjectives()
    {
        objectives = new List<Objective>();

        int ammount = UnityEngine.Random.Range(3, 5);
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        Bounds bounds = ground.GetComponent<TerrainCollider>().bounds;

        for ( int i = 0; i < ammount; i++ )
        {
            Vector3 position = new Vector3
            (
                bounds.center.x + UnityEngine.Random.Range(-bounds.extents.x / 2, bounds.extents.x / 2),
                bounds.center.y + bounds.extents.y / 2 + 1,
                bounds.center.z + UnityEngine.Random.Range(-bounds.extents.z / 2, bounds.extents.z / 2)
            );

            // Adjust to terrain hit
            Ray ray = new Ray(position, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject go = Instantiate(objectivePrefab, hitInfo.point, Quaternion.identity) as GameObject;
                objectives.Add(go.GetComponentOrEnd<Objective>());
                go.transform.SetParent(objectivesParent.transform);
            }
        }
    }

	private void moveUnits(GameObject target)
	{
        if (selectedUnits.hasMovableUnits())
        {
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
                }

                UnitMovement script = unit.GetComponentInParent<UnitMovement>();
                if (script != null)
                {
                    CollectResources collect = unit.GetComponent<CollectResources>();
                    if (collect != null && AI.Instance.resources.Contains(target.tag))
                    {
                        collect.startMovingToCollect(target);
                        collect.targetToCollect = target;
                    }
                    else
                    {
			            script.startMoving(target);
                        target.GetComponent<timerDeath>().AddUnit(unit);
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
                allAllyCivilians.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                allAllyBuildings.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                allAllyArmy.Remove(u);
            }
        }
        if (u.tag == "Enemy")
        {
            if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Civilian)
            {
                allEnemyCivilians.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.TownCenter
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Barracs
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archery
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Stable)
            {
                allEnemyBuildings.Remove(u);
            }
            else if (u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Soldier
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Archer
              || u.gameObject.GetComponentOrEnd<Identity>().unitType == UnitType.Knight)
            {
                allEnemyArmy.Remove(u);
            }
        }
        checkWin();
        checkLose();

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
        playerResources[Resource.Food] = 1000;
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
        hud.updateResource(Resource.Population, playerResources[Resource.Population]);
    }

    //Called to check whether there are enough resources to perform an action.
    //Params: resource type, amount (to subtract).
    //Returns true if there are enough, and updates the resource type.
    //Returns false if there aren't enough, and displays warnings.
    public bool checkResources(ResourceValueDictionary resourceCosts, String player)
    {
        ResourceValueDictionary resDict;
        if (player == "player") resDict = playerResources;
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
        if (player == "player") resDict = playerResources;
        else resDict = cpuResources;
        playerResources[res] -= value;
        hud.updateResource(res, resDict[res]); //- value);  Per què es mostra un resource que no és el que hi ha?
    }

    public void updateResource(Resource res, int value)
    {
        playerResources[res] -= value;
        hud.updateResource(res, playerResources[res]); //- value);  Per què es mostra un resource que no és el que hi ha?
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

    public void checkMapControl()
    {
        hud.updateSelection(selectedUnits);
        Civilization possibleWinner = objectives[0].Controller;
        foreach(Objective objective in objectives)
        {
            if (objective.Controller != possibleWinner)
                return;
        }

        if (possibleWinner != Civilization.Neutral)
        {
            hud.startCountdown(Victory.MapControl, possibleWinner);
            InvokeRepeating("ensureWinner", 1, 1);
        }
    }

    public void ensureWinner()
    {
        Civilization possibleWinner = objectives[0].Controller;
        foreach (Objective objective in objectives)
        {
            if (objective.Controller != possibleWinner)
            {
                hud.stopCountdown(Victory.MapControl);
                return;
            }
                
        }
    }
    public void checkWin()
    {
        if (allEnemyUnits.Count == 0) winCondition();
    }

    public void checkLose()
    {
        if (allAllyUnits.Count == 0) loseCondition();
    }

    //Ends the game.
    private void winCondition()
	{
        //!!!hud.ShowWinMessage();
    }

    private void loseCondition()
    {
        //!!!hud.ShowLoseMessage();
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
        
        /* adjust spawning point
        Ray ray = new Ray(building.transform.position + 5 * building.transform.up + 10 * building.transform.forward, -Vector3.up);
        
        bool freeSpaceFound = false;

        RaycastHit hitInfo = new RaycastHit();

        while (!freeSpaceFound)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag != "Ground")
                {
                    ray.origin = ray.origin + Vector3.right * 2;
                }
                else freeSpaceFound = true;
            }
        }*/


        GameObject newUnit = Instantiate(unit, spawningPoint, Quaternion.identity) as GameObject;
        addSelectedPrefab(newUnit);
        addTeamCirclePrefab(newUnit);
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
    

    public GameObject createBuilding(GameObject prefab)
	{
        //Instantiate the building and start the positioning of the building
        GameObject building = Instantiate (prefab, Vector3.zero, gameObject.transform.rotation) as GameObject;

        building.tag = "Ally";

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
                unit.GetComponent<Construct>().SetBuildingToConstruct(building);


            }
        }
        
        building.AddComponent<BuildingPlacer> ();

        enabled = false;
    
        return building;  

        //createBuilding(prefab, new Vector3(213, -5, 141));   
	}


    public void createBuilding(GameObject prefab, Vector3 position, Troop t)
    {
        GameObject building = Instantiate(prefab, position, gameObject.transform.rotation) as GameObject;
        building.tag = "Ally";
        addSelectedPrefab(building);
        addTeamCirclePrefab(building);

        building.GetComponent<BuildingConstruction>().setConstructionOnGoing(true);

        foreach (var unit in t.units) unit.GetComponent<Construct>().SetBuildingToConstruct(building);

        buildingConstruction(position, t);

    }

    public void buildingConstruction(Vector3 position, Troop t)
    {
        //Move the units that are selected to construct to the building position
        GameObject target = Instantiate(targetPrefab, position, Quaternion.identity) as GameObject;
        target.transform.SetParent(targetsParent.transform);
        moveUnits(target, t);

        //Order that the unit has to construct
        foreach (var unit in t.units) unit.GetComponent<Construct>().setConstruct(true);

        enabled = true;//Enable the script 
    }

    // Action events
    public void OnActionButtonEnter(UnitData data)
    {
        hud.OnActionButtonEnter(data);
    }

    public void OnActionButtonExit(UnitData data)
    {
        hud.OnActionButtonExit(data);
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

    public void OnCreate( Identity who, UnitType what )
    {
        // get the unit data and the prefab of the unit that can be created
        GameObject prefab = DataManager.Instance.civilizationDatas[who.civilization].units[what];
        UnitData unitData = DataManager.Instance.unitDatas[what];
        //print("Creating " + unitData.description);

        if (what.isBuilding())
        {
            if (checkResources(unitData.resourceCost, "player"))
            {
                //Create the building
                GameObject created = createBuilding(prefab);
                created.tag = who.gameObject.tag;
                Spawner spa = created.GetComponent<Spawner>();
                if (spa != null) spa.initBounds();
                updateResource(unitData.resourceCost, "player");
            }
        }
        else
        {
            Action action = new Action(unitData.preview, unitData.requiredTime, () =>
            {
                GameObject created = CreateUnit(who.gameObject, prefab);
                created.tag = who.gameObject.tag;
                hud.updateDelayedActions(selectedUnits.FocusedUnit);
            });
            //create an action and add it to the focused unit's queue
            if (who.gameObject.GetComponentOrEnd<DelayedActionQueue>().Enqueue(action)) {
                if (checkResources(unitData.resourceCost, "player"))
                {
                    //DelayedActionQueue script = who.gameObject.GetComponentOrEnd<DelayedActionQueue>();
                    //script.Enqueue(action);
                    updateResource(unitData.resourceCost, "player");
                    if (who.gameObject.tag == "Ally") hud.updateDelayedActions(selectedUnits.FocusedUnit);
                }
            }
        }
        
     }

    public void addSelectedPrefab(GameObject go)
    {
        GameObject selectedProj = Instantiate(selectedPrefab, go.transform.position + new Vector3(0,5,0), Quaternion.identity) as GameObject;
        //selectedProj.transform.Rotate(90, 0, 0);
        selectedProj.name = "Selected";
        selectedProj.SetActive(false);
        selectedProj.transform.SetParent(go.transform);
        selectedProj.transform.up = new Vector3(0,0,1);
        SelectionCircle script = selectedProj.GetComponent<SelectionCircle>();
        if (script != null) script.init();
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
        GameObject unit = GameController.Instance.selectedUnits.FocusedUnit;
        Health health = unit.GetComponent<Health>();

        if(health != null )
        {
            health.die();
        }
        else
        {
            GameController.Instance.hud.showMessageBox("Not implemented");
        }
    }

    public void OnAttack()
    {
        print("attack");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }

    public List<GameObject> getAllAllyArmy()
    {
        return allAllyArmy;
    }
    public List<GameObject> getAllAllyBuildings()
    {
        return allAllyBuildings;
    }
    public List<GameObject> getAllAllyCivilians()
    {
        return allAllyCivilians;
    }
    public List<GameObject> getAllEnemyArmy()
    {
        return allEnemyArmy;
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
