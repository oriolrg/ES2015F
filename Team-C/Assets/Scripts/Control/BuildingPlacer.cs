using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour {

	private Color originalColor;
	private Color red = new Color(1f, 0f, 0f, 0.5f);


	void OnEnable()
	{
		originalColor = gameObject.GetComponent<Renderer> ().material.color;
		
		gameObject.GetComponent<Renderer> ().material.color = new Color(
			originalColor.r, originalColor.g, originalColor.b, 0.5f
		);
	}
	
	// Update is called once per frame
	void Update () {
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
			else if (!hit.collider.gameObject.Equals(gameObject))
			{
				collidingWithObjects = true;
			}
		}
		
		if (groundHitFound)
		{
			gameObject.transform.position = new Vector3 (
				groundHit.point.x, gameObject.transform.position.y, groundHit.point.z
			);
			
			//When the left button of the mouse is clicked, get the position and create a prefab there.
			if (collidingWithObjects)
			{
				Color color = gameObject.GetComponent<Renderer> ().material.color;
				//provisionalPrefab.GetComponent<Renderer> ().material.SetColor ("_Emission", new Color(1f, 0f, 0f, 1f));
				
				gameObject.GetComponent<Renderer> ().material.color = new Color(1f, 0f, 0f, 0.5f);
			} 
			else if(Input.GetKeyDown (KeyCode.Mouse0)) 
			{
				gameObject.GetComponent<Renderer> ().material.color = originalColor;

				GameController.Instance.enabled = true;
				enabled = false;
				Destroy (this);
				
			} else {
				gameObject.GetComponent<Renderer> ().material.color = new Color(
					originalColor.r, originalColor.g, originalColor.b, 0.5f
				);
			}
		}
	
	}
}
