using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject unitsParent;

    [SerializeField] private GameObject buildingsParent;
    [SerializeField]
	private Troop selectedUnits;

    //Keep track of all allied/enemy units, add and remove with addUnit and removeUnit.
    private List<GameObject> allAllyUnits;
    private List<GameObject> allEnemyUnits;

    //Keeps track of the resources the player has.
    [SerializeField]
    private ResourceValueDictionary resourceDict;

    [SerializeField]
    private GameObject selectedPrefab;
    [SerializeField]
	private GameObject targetPrefab;

    [SerializeField]
    private float UIheight;

    public HUD hud;

	private bool isSelecting;
	private Vector3 mPos;

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
        selectedUnits = new Troop();
        selectedUnits.units = new List<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        selectedUnits = new Troop();
        initResourceValues();
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
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (hitInfo.transform.gameObject.tag == "Ally" || hitInfo.transform.gameObject.tag == "StorageFood")
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
                    if (hitInfo.transform.gameObject.tag == "Food")
                    {
                        target = Instantiate(targetPrefab, hitInfo.transform.gameObject.transform.position, Quaternion.identity) as GameObject;
                        //moveUnitsCollect(target);
                    }
                    else if(hitInfo.transform.gameObject.tag == "Ally" && hitInfo.transform.gameObject.GetComponent<BuildingConstruction>().getConstructionOnGoing())
                    {
                        Debug.Log("vaig a construir");

                        Troop troop = new Troop(selectedUnits.units);

                        foreach (var unit in troop.units)
                        {
                            if (unit.GetComponent<Construct>().getConstruct() || unit.GetComponent<Construct>().getInConstruction())
                            {
                                unit.GetComponent<Construct>().setConstruct(false);
                                unit.GetComponent<Construct>().SetInConstruction(false);
                                unit.GetComponent<Construct>().getBuildingToConstruct().GetComponent<BuildingConstruction>().deleteUnit(unit);
                            }

                            unit.GetComponent<Construct>().SetBuildingToConstruct(hitInfo.transform.gameObject);
                        }

                        buildingConstruction(hitInfo.transform.gameObject.transform.position, troop);
                        
                    }
                    else
                    {
                        target = Instantiate(targetPrefab, hitInfo.point, Quaternion.identity) as GameObject;
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

	private void moveUnits(GameObject target)
	{
        if (selectedUnits.hasMovableUnits())
        {
            foreach (var unit in selectedUnits.units)
            {
                
                if (unit.GetComponent<Construct>().getConstruct() || unit.GetComponent<Construct>().getInConstruction())
                {
                    unit.GetComponent<Construct>().setConstruct(false);
                    unit.GetComponent<Construct>().SetInConstruction(false);
                    unit.GetComponent<Construct>().getBuildingToConstruct().GetComponent<BuildingConstruction>().deleteUnit(unit);
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
            foreach (var unit in selectedUnits.units)
            {
                StaticUnit script = unit.GetComponentInParent<StaticUnit>();
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

                if (unit.GetComponent<Construct>().getConstruct() || unit.GetComponent<Construct>().getInConstruction())
                {
                    unit.GetComponent<Construct>().setConstruct(false);
                    unit.GetComponent<Construct>().SetInConstruction(false);
                    unit.GetComponent<Construct>().getBuildingToConstruct().GetComponent<BuildingConstruction>().deleteUnit(unit);
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
                StaticUnit script = unit.GetComponentInParent<StaticUnit>();
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
        if (u.tag == "Ally") allAllyUnits.Add(u);
        if (u.tag == "Enemy") allEnemyUnits.Add(u);
    }

    public void removeUnit(GameObject u)
    {
        if (u.tag == "Ally") allAllyUnits.Remove(u);
        if (u.tag == "Enemy") allEnemyUnits.Remove(u);
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
        resourceDict[Resource.Food] = 1000;
        resourceDict[Resource.Wood] = 1000;
        resourceDict[Resource.Metal] = 1000;
        resourceDict[Resource.Population] = 10;
        hud.updateResource(Resource.Food, resourceDict[Resource.Food]);
        hud.updateResource(Resource.Wood, resourceDict[Resource.Wood]);
        hud.updateResource(Resource.Metal, resourceDict[Resource.Metal]);
        hud.updateResource(Resource.Population, resourceDict[Resource.Population]);
    }

    //Called to check whether there are enough resources to perform an action.
    //Params: resource type, amount (to subtract).
    //Returns true if there are enough, and updates the resource type.
    //Returns false if there aren't enough, and displays warnings.
    public bool checkResources(ResourceValueDictionary resourceCosts)
    {
        bool check = true;
        foreach (KeyValuePair<Resource, int> kv in resourceCosts)
        {
            if (resourceDict[kv.Key] - kv.Value < 0)
            {
                //Here goes stuff that happens when there aren't enough resources to perform the action.
                //i.e. text pop-up, sound warning.
                check = false;
            }
        }
        if (check)
        {
            foreach (KeyValuePair<Resource, int> kv in resourceCosts)
            {
                updateResource(kv.Key, kv.Value);
            }
        }
        return check;
    }

    public void updateResource(Resource res, int value)
    {
        resourceDict[res] -= value;
        hud.updateResource(res, resourceDict[res] - value);    
    }

    public void updateInteractable()
    {
        hud.updateInteractable(selectedUnits.FocusedUnit);
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

        Vector3 spawningPoint = spawner.spawningPoint;
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
        // Set unit as parent in hierarchy
        newUnit.transform.SetParent(unitsParent.transform);
        GameObject target = Instantiate(targetPrefab, spawner.rallyPoint, Quaternion.identity) as GameObject;
        UnitMovement script = newUnit.GetComponent<UnitMovement>();

        if (script != null)
        {
            script.startMoving(target);
            target.GetComponent<timerDeath>().AddUnit(newUnit);
        }
        return newUnit;
    }
    

    public void createBuilding(GameObject prefab)
	{
        //Instantiate the building and start the positioning of the building

        GameObject building = Instantiate (prefab, Vector3.zero, gameObject.transform.rotation) as GameObject;

        addSelectedPrefab(building);

        
        building.GetComponent<BuildingConstruction>().setConstructionOnGoing(true);
        
        updateInteractable();

        //foreach (var unit in selectedUnits.units) unit.GetComponent<Construct>().SetBuildingToConstruct(building);
        
        building.AddComponent<BuildingPlacer> ();

        enabled = false;     
        //createBuilding(prefab, new Vector3(213, -5, 141));   
	}


    public void createBuilding(GameObject prefab, Vector3 position, Troop t)
    {
        GameObject building = Instantiate(prefab, position, gameObject.transform.rotation) as GameObject;

        addSelectedPrefab(building);

        building.GetComponent<BuildingConstruction>().setConstructionOnGoing(true);

        //foreach (var unit in t.units) unit.GetComponent<Construct>().SetBuildingToConstruct(building);

        buildingConstruction(position, t);

    }

    public void buildingConstruction(Vector3 position, Troop t)
    {
        //Move the units that are selected to construct to the building position
        GameObject target = Instantiate(targetPrefab, position, Quaternion.identity) as GameObject;
        moveUnits(target, t);

        //Order that the unit has to construct
        //foreach (var unit in t.units) unit.GetComponent<Construct>().setConstruct(true);

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
            //Create the building
            createBuilding(prefab);
        }
        else
        {
            //create an action and add it to the focused unit's queue

            Action action = new Action(unitData.preview, unitData.requiredTime, () => 
            {
                CreateUnit(who.gameObject, prefab);
                hud.updateDelayedActions(selectedUnits.FocusedUnit);
            });

            DelayedActionQueue script = selectedUnits.FocusedUnit.GetComponentOrEnd<DelayedActionQueue>();

            script.Enqueue(action);

            print("Creating " + unitData.description);
            hud.updateDelayedActions(selectedUnits.FocusedUnit);

        }
     }

    public void addSelectedPrefab(GameObject go)
    {
        GameObject selectedProj = Instantiate(selectedPrefab, go.transform.position + new Vector3(0,5,0), Quaternion.identity) as GameObject;
        //selectedProj.transform.Rotate(90, 0, 0);
        selectedProj.SetActive(false);
        selectedProj.transform.SetParent(go.transform);
        selectedProj.transform.up = new Vector3(0,0,1);
        SelectionCircle script = selectedProj.GetComponent<SelectionCircle>();
        if (script != null) script.init();

    }

    public void OnSacrifice()
    {
        print("sacrifice");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }

    public void OnAttack()
    {
        print("attack");
        GameController.Instance.hud.showMessageBox("Not implemented");
    }


}
