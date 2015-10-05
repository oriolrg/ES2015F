using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {

    public GameObject prefab;
	[SerializeField] private GameObject provisionalPrefab;
	

	void OnEnable()
	{
		provisionalPrefab = Instantiate (prefab, Vector3.zero, prefab.transform.rotation) as GameObject;
		Color color = provisionalPrefab.GetComponent<Renderer> ().material.color;
		
		provisionalPrefab.GetComponent<Renderer> ().material.color = new Color(color.r, color.g, color.b, 0.5f);

        
	}


    void Update()
    {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);

		bool collidingWithObjects = false, groundHitFound = false;
		RaycastHit groundHit;

		foreach (RaycastHit hit in hits) 
		{
			if (hit.collider.gameObject.tag == "Ground")
			{
				groundHit = hit; groundHitFound = true;
			}
			else if (!hit.collider.gameObject.Equals(provisionalPrefab))
			{
				collidingWithObjects = true;
			}
		}

		if (groundHitFound)
		{
			provisionalPrefab.transform.position = new Vector3 (groundHit.point.x, prefab.transform.position.y, groundHit.point.z);
			
			//When the left button of the mouse is clicked, get the position and create a prefab there.
			if (collidingWithObjects)
			{
				Color color = provisionalPrefab.GetComponent<Renderer> ().material.color;
				//provisionalPrefab.GetComponent<Renderer> ().material.SetColor ("_Emission", new Color(1f, 0f, 0f, 1f));
				
				provisionalPrefab.GetComponent<Renderer> ().material.color = new Color(1f, 0f, 0f, 1f);

			} 
			else if(Input.GetKeyDown (KeyCode.Mouse0)) 
			{
				Color color = provisionalPrefab.GetComponent<Renderer> ().material.color;
				
				provisionalPrefab.GetComponent<Renderer> ().material.color = new Color(color.r, color.g, color.b,1f);
				
				GetComponent<GameController> ().enabled = true;
				enabled = false;
				
			}
		}

    }
}
