using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {
	[SerializeField]
	private Troop selectedUnits;

    //Keep track of all allied/enemy units, add and remove with addUnit and removeUnit.
    private List<GameObject> allAllyUnits;
    private List<GameObject> allEnemyUnits;

    [SerializeField]
	private GameObject targetPrebab;

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

        allAllyUnits = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addUnit(go);

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

                        //selectedGO.GetComponent<Focusable>().onFocus();
                        if (!selectedUnits.units.Contains(selectedGO)) selectedUnits.units.Add(selectedGO);
                        selectedUnits.FocusedUnit = selectedGO;
                        selectedGO.transform.Find("Selected").gameObject.SetActive(true);
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
                    GameObject target = Instantiate(targetPrebab, hitInfo.point, Quaternion.identity) as GameObject;
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
                            //unit.transform.Find("Selected").gameObject.SetActive(true);
                        }
                        if (selectedUnits.units.Count > 0) selectedUnits.FocusedUnit = selectedUnits.units[0];
                        hud.updateSelection(selectedUnits);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //if (selectedUnits.units.Count > 0) { ... }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            for (int i = allAllyUnits.Count - 1; i >= 0; i--)
            {
                Animator a = allAllyUnits[i].GetComponent<Animator>();
                if (a == null)
                    a = allAllyUnits[i].GetComponentInParent<Animator>();
                
                a.SetBool("dead", true);
            }
            Debug.LogError("MM");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Camera.main.transform.position = new Vector3(85, 13, -25);
            Debug.Log("Future models");
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

            //Move the units only if they are not constructing a building
            if (!unit.GetComponent<Unit>().getInConstruction())
            {
                if (unit.GetComponent<Unit>().getConstruct())
                {
                    unit.GetComponent<Unit>().setConstruct(false);
                }

                unit.GetComponentInParent<UnitMovement>().startMoving(target);
                target.GetComponent<timerDeath>().AddUnit(unit);

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
        GameController.Instance.checkWin();
        GameController.Instance.checkLose();
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
                unit.transform.Find("Selected").gameObject.SetActive(false);
            }
            selectedUnits.units.Clear();
            selectedUnits.FocusedUnit = null;
            //hud.Clear();
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
        GameObject target = Instantiate(targetPrebab, position, Quaternion.identity) as GameObject;
        moveUnits(target);

        enabled = true;//Enable the script 

        //Order that the unit has to construct
        foreach (var unit in selectedUnits.units) unit.GetComponent<Unit>().setConstruct(true);

    }
}
