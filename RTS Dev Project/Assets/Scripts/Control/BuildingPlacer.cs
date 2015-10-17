using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour {

	private Color originalColor;
    private Color transparentColor;
	private Color red = new Color(1f, 0f, 0f, 0.5f);

    private bool collision;//indicates if there is a collision
	private int counterCollision;//indicates how many different collisions there are


	void OnEnable()
	{
		originalColor = gameObject.GetComponent<Renderer> ().material.color;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        
        //Make the gameObject a bit transparent
        gameObject.GetComponent<Renderer> ().material.color = transparentColor;

        collision = false;
		counterCollision = 0;
    }

    void OnTriggerEnter(Collider col)
    {
		counterCollision++;
        collision = true;

    }

    void OnTriggerExit(Collider col)
    {

		counterCollision--;
		if (counterCollision == 0) {
			collision = false;
		}

    }
	

	// Update is called once per frame
	void Update () {

        //Cancel the building creation with mouse right button
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(gameObject);
            
            GameController.Instance.enabled = true;

            enabled = false;

            Destroy(this);
        }


        //Create a ray and look for all collisions and keep the ground collision
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		
		bool groundHitFound = false;
		RaycastHit groundHit = default(RaycastHit); // ignore uninitialized error
		
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
			else if(Input.GetKeyUp (KeyCode.Mouse0))  
			{
                //When there is no collision and the mouse left button is clicked, order to start the construction

                gameObject.GetComponent<Renderer> ().material.color = transparentColor;

                //GameController.Instance.enabled = true;

                GameController.Instance.buildingConstruction(gameObject.transform.position);

                enabled = false;
				Destroy (this);
				
			} else {
                //GameObject original color with transparency

				gameObject.GetComponent<Renderer> ().material.color = transparentColor;
			}
		}
	
	}
}
