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
					}
					else
					{
						selectedUnits.Clear();
						selectedUnits.Add(selectedGO);
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
			var camera = Camera.main;
			var viewportBounds = RectDrawer.GetViewportBounds( camera, mPos, Input.mousePosition );
			if (!Input.GetKey(KeyCode.LeftControl)) selectedUnits.Clear();
			foreach( var aux in FindObjectsOfType<GameObject>() )
			{
				if (viewportBounds.Contains(camera.WorldToViewportPoint(aux.transform.position )) & aux.tag=="Ally" & !selectedUnits.Contains(aux))
					selectedUnits.Add(aux);
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
