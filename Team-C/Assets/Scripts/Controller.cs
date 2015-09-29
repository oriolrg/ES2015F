using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
	[SerializeField]
	private List<GameObject> selectedUnits;

	private bool isSelecting;
	private Vector3 mPos;
	
	// Use this for initialization
	void Start () {
		selectedUnits = new List<GameObject> ();;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			isSelecting = true;
			mPos = Input.mousePosition;
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{

				GameObject selectedGO = hitInfo.transform.gameObject;
				if (hitInfo.transform.gameObject.tag == "Ally")
				{                   
					if (Input.GetKey(KeyCode.LeftControl))
					{
						if (!selectedUnits.Contains(selectedGO)) selectedUnits.Add(selectedGO);
						selectedGO.transform.Find("Selected").gameObject.SetActive(true);
					}
					else
					{
						foreach (var unit in selectedUnits) {
							unit.transform.Find("Selected").gameObject.SetActive(false);
						}
						selectedUnits.Clear();
						selectedUnits.Add(selectedGO);
						selectedGO.transform.Find("Selected").gameObject.SetActive(true);
					}
				}
				else
				{
					Debug.Log("not Ally");
				}
			}
			else
			{
				Debug.Log("No hit");
			}
		}

		if( Input.GetMouseButtonUp( 0 ) )
		{		
			isSelecting = false;
			if ((mPos - Input.mousePosition).magnitude>5){
				var camera = Camera.main;
				var viewportBounds = RectDrawer.GetViewportBounds( camera, mPos, Input.mousePosition );
				//Deselecting
				foreach (var unit in selectedUnits) {
					unit.transform.Find("Selected").gameObject.SetActive(false);
				}
				if (!Input.GetKey(KeyCode.LeftControl)) selectedUnits.Clear();

				//Selecting
				foreach( var unit in FindObjectsOfType<GameObject>() )
				{
					if (viewportBounds.Contains(camera.WorldToViewportPoint(unit.transform.position )) & unit.tag=="Ally" & !selectedUnits.Contains(unit)) {
						selectedUnits.Add(unit);
						unit.transform.Find("Selected").gameObject.SetActive(true);
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if( isSelecting )
		{
			var rect = RectDrawer.GetScreenRect(mPos, Input.mousePosition );
			RectDrawer.DrawScreenRect( rect, new Color( 0.6f, 0.9f, 0.6f, 0.25f ) );
		}
	}


}
