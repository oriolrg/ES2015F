using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	[SerializeField]
	private List<GameObject> selectedUnits;
    private List<GameObject> allAllyUnits;

	[SerializeField]
	private GameObject targetPrebab;

    public IngameHUD hud;

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

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        // not now!!! DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
		selectedUnits = new List<GameObject> ();

        allAllyUnits = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ally")) addAllyUnit(go);

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
                    print("Ally");
                    if (!Input.GetKey(KeyCode.LeftControl)) ClearSelection();
				
                    selectedGO.GetComponent<Focusable>().onFocus();
                    if (!selectedUnits.Contains(selectedGO)) selectedUnits.Add(selectedGO);
                    selectedGO.transform.Find("Selected").gameObject.SetActive(true);
				}
				else
				{
					Debug.Log("not Ally");
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

		//End of click
		if( Input.GetMouseButtonUp( 0 ) )
		{
            if (isSelecting)
            {
                isSelecting = false;

                //We impose a size of 5 to detect a box.
                //Box Selection
                if ((mPos - Input.mousePosition).magnitude > 5)
                {
                    var camera = Camera.main;
                    var viewportBounds = RectDrawer.GetViewportBounds(camera, mPos, Input.mousePosition);

                    //Deselecting
                    if (!Input.GetKey(KeyCode.LeftControl)) ClearSelection();

                    //Selecting
                    foreach (var unit in FindObjectsOfType<GameObject>())
                    {
                        //Units inside the rect get selected.
                        if (viewportBounds.Contains(camera.WorldToViewportPoint(unit.transform.position)) & unit.tag == "Ally" & !selectedUnits.Contains(unit))
                        {
                            selectedUnits.Add(unit);
                            unit.transform.Find("Selected").gameObject.SetActive(true);
                        }
                    }
                }
            }
	    }
        if(Input.GetKeyDown(KeyCode.M))
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
			var rect = RectDrawer.GetScreenRect(mPos, Input.mousePosition );
			RectDrawer.DrawScreenRect( rect, new Color( 0.6f, 0.9f, 0.6f, 0.25f ) );
		}
	}

	private void moveUnits(GameObject target)
	{
		foreach (var unit in selectedUnits) 
		{
			unit.GetComponentInParent<UnitMovement>().startMoving(target);
			target.GetComponent<timerDeath>().AddUnit(unit);
		}
	}

    public void addAllyUnit(GameObject u)
    {
        allAllyUnits.Add(u);
        //Debug.Log("Unit added, total units: " + allAllyUnits.Count);
    }

    public void removeAllyUnit(GameObject u)
    {
        allAllyUnits.Remove(u);
        Debug.Log("Unit removed, total units: " + allAllyUnits.Count);
        GameController.Instance.checkLose();
    }

    public void checkLose()
    {
        if (allAllyUnits.Count == 0) loseCondition();
    }

    //Ends the game.
    private void winCondition()
	{
        //hud.ShowWinMessage();
    }

    private void loseCondition()
    {
        hud.ShowLoseMessage();
    }

    public void reloadLevel()
    {
        Application.Quit();
    }

    // Called when selected units are destroyed
    public void ClearSelection()
    {
        foreach (var unit in selectedUnits)
        {
            unit.transform.Find("Selected").gameObject.SetActive(false);
        }
        selectedUnits.Clear();
        hud.Clear();
    }

	public void createBuilding(GameObject prefab)
	{
        // old code with ClickController
        //		clickController.prefab = prefab;
        //		clickController.enabled = true;

        // new code with BuildingPlacer

        

		GameObject building = Instantiate (prefab, Vector3.zero, gameObject.transform.rotation) as GameObject;

        foreach (var unit in selectedUnits) unit.GetComponent<Villager>().setBuildingToConstruct(building);

        building.AddComponent<BuildingPlacer> ().enabled = true;

       
        enabled = false;
        
	}

    public void buildingConstruction(Vector3 position)
    {

        GameObject target = Instantiate(targetPrebab, position, Quaternion.identity) as GameObject;
        moveUnits(target);

        //guardar array de units que estan construint per bloquejarles ¿?


    }
}
