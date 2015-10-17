using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	[SerializeField]
	private Troop selectedUnits;

    //Keep track of all allied/enemy units, add and remove with addUnit and removeUnit.
    private List<GameObject> allAllyUnits;
    private List<GameObject> allEnemyUnits;

    //Keeps track of the resources the player has.
    [SerializeField]
    private ResourceValueDictionary resourceDict;

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
	void Update ()
    {
		//Win Condition
		var wonder = GameObject.FindGameObjectWithTag("Wonder");
		if (wonder != null) 
		{
			winCondition ();
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
                if (hit)
                {
                    GameObject target = Instantiate(targetPrefab, hitInfo.point, Quaternion.identity) as GameObject;
                    moveUnits(target);
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
                            if( projector != null)
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
		foreach (var unit in selectedUnits.units) 
		{
            Debug.Log("presente");
            //Move the units only if they are not constructing a building
            if (!unit.GetComponent<Unit>().getInConstruction())
            {
                if (unit.GetComponent<Unit>().getConstruct())
                {
                    unit.GetComponent<Unit>().setConstruct(false);
                }

                UnitMovement script = unit.GetComponentInParent<UnitMovement>();
                if (script != null)
                {
                    script.startMoving(target);
                    target.GetComponent<timerDeath>().AddUnit(unit);
                }

                TownCenter TC = unit.GetComponentInParent<TownCenter>();
                if (TC != null)
                {
                    TC.setRallyPoint(target.transform.position);
                }
            }
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

        if(selectedUnits.FocusedUnit == u)
        {
            hud.ClearSelection();
        }
    }

    //Set starting resource values.
    public void initResourceValues()
    {
        resourceDict[Resource.Food] = 100;
        resourceDict[Resource.Wood] = 100;
        resourceDict[Resource.Metal] = 100;
        resourceDict[Resource.Population] = 2;
        hud.updateResource(Resource.Food, resourceDict[Resource.Food]);
        hud.updateResource(Resource.Wood, resourceDict[Resource.Wood]);
        hud.updateResource(Resource.Metal, resourceDict[Resource.Metal]);
        hud.updateResource(Resource.Population, resourceDict[Resource.Population]);
    }

    //Called to check whether there are enough resources to perform an action.
    //Params: resource type, amount (negative amount to subtract, positive to add).
    //Returns true if there are enough, and updates the resource type.
    //Returns false if there aren't enough, and displays warnings.
    public bool checkResources(Resource res, int value)
    {
        if (resourceDict[res] + value < 0)
        {
            //Here goes stuff that happens when there aren't enough resources to perform the action.
            //i.e. text pop-up, sound warning.
            return false;
        }
        updateResource(res, value);
        return true;
    }

    public void updateResource(Resource res, int value)
    {
        resourceDict[res] += value;
        hud.updateResource(res, resourceDict[res]);
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
                unit.transform.FindChild("Selected").gameObject.SetActive(false);
            }
            selectedUnits.units.Clear();
            selectedUnits.FocusedUnit = null;
            hud.ClearSelection();
        }
    }

    public void CreateUnit(Transform buildingTrans, GameObject prefab, Vector3 rally)
    {
        Ray ray = new Ray(buildingTrans.position - 3 * buildingTrans.up + 10 * buildingTrans.forward, -Vector3.up);

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
        }

        print(freeSpaceFound);

        GameObject newUnit = Instantiate(prefab, hitInfo.point, Quaternion.identity) as GameObject;
        GameObject target = Instantiate(targetPrefab, rally, Quaternion.identity) as GameObject;
        UnitMovement script = newUnit.GetComponent<UnitMovement>();
        
        if (script != null)
        {
            script.startMoving(target);
            //target.GetComponent<timerDeath>().AddUnit(newUnit);
        }
    }

    public void createBuilding(GameObject prefab)
	{
        //Instantiate the building and start the positioning of the building

		GameObject building = Instantiate (prefab, Vector3.zero, gameObject.transform.rotation) as GameObject;

        foreach (var unit in selectedUnits.units) unit.GetComponent<Civil>().SetBuildingToConstruct(building);

        building.AddComponent<BuildingPlacer> ().enabled = true;

        enabled = false;        
	}

    public void buildingConstruction(Vector3 position)
    {
        //Move the units that are selected to construct to the building position
        GameObject target = Instantiate(targetPrefab, position, Quaternion.identity) as GameObject;
        moveUnits(target);

        enabled = true;//Enable the script 

        //Order that the unit has to construct
        foreach (var unit in selectedUnits.units) unit.GetComponent<Unit>().setConstruct(true);

    }
}
