using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
	[SerializeField]
	private List<GameObject> selectedUnits;
	
	// Use this for initialization
	void Start () {
		selectedUnits = new List<GameObject> ();;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
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
	}
}
