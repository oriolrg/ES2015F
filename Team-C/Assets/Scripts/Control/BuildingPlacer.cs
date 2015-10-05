using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour {

	private Color originalColor;
	private Color red = new Color(1f, 0f, 0f, 0.5f);

    private bool collision;//indicates if there is a collision


	void OnEnable()
	{
		originalColor = gameObject.GetComponent<Renderer> ().material.color;
		
        //Make the gameObject a bit transparent
		gameObject.GetComponent<Renderer> ().material.color = new Color(
			originalColor.r, originalColor.g, originalColor.b, 0.5f
		);

        collision = false;
    }

    void OnTriggerEnter(Collider col)
    {

        collision = true;

    }

    void OnTriggerExit(Collider col)
    {

        collision = false;

    }
	

	// Update is called once per frame
	void Update () {

        //Create a ray and look for all collisions and keep the ground collision
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		
		bool groundHitFound = false;
		RaycastHit groundHit;
		
		foreach (RaycastHit hit in hits) 
		{
			if (hit.collider.gameObject.tag == "Ground")
			{
				groundHit = hit; groundHitFound = true;
                break;
			}
			
		}
		
		if (groundHitFound) 
		{
            //Move the game object to the mouse position, which is the ray hit position with the ground

            gameObject.transform.position = new Vector3 (
				groundHit.point.x, gameObject.transform.position.y, groundHit.point.z
			);
			
            if(collision)
            {
                // Change the gameObject color to red, indicating that it is not posible to create the building there

                gameObject.GetComponent<Renderer> ().material.color = red;

			} 
			else if(Input.GetKeyDown (KeyCode.Mouse0))  
			{
                //When there is no collision and the mouse left button is clicked, remove the gameObject transparency and unable the script

                gameObject.GetComponent<Renderer> ().material.color = originalColor;

				GameController.Instance.enabled = true;
				enabled = false;
				Destroy (this);
				
			} else {
                //GameObject original color with transparency

				gameObject.GetComponent<Renderer> ().material.color = new Color(
					originalColor.r, originalColor.g, originalColor.b, 0.5f
				);
			}
		}
	
	}
}
