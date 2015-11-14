using UnityEngine;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour {

	private Color originalColor;
    private Color transparentColor;
	private Color red = new Color(1f, 0f, 0f, 0.5f);
    private List<Material> originalMaterials;

    private bool collision;//indicates if there is a collision
	private int counterCollision;//indicates how many different collisions there are


	void Start()
	{

		originalColor = gameObject.GetComponent<Renderer> ().material.color;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

		gameObject.GetComponent<LOSEntity>().enabled = false;

        //Make the gameObject a bit transparent. Hacky hacky
        originalMaterials = new List<Material>();
        foreach (Material material in GetComponent<Renderer>().materials)
        {
            Material originalMaterial = new Material(material);
            originalMaterials.Add(originalMaterial);

            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.color = transparentColor;
        }
        collision = false;
		counterCollision = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag != "Ground")
        {
            counterCollision++;
            collision = true;

        }
		

    }

    void OnTriggerExit(Collider col)
    {
		if(col.gameObject.tag != "Ground")
        {
            counterCollision--;
            if (counterCollision == 0)
            {
                collision = false;
            }
        }

    }
	

	// Update is called once per frame
	void Update ()
    {

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

            gameObject.transform.position = groundHit.point;
			
            if(collision)
            {
                // Change the gameObject color to red, indicating that it is not posible to create the building there
                foreach(Material material in GetComponent<Renderer>().materials)
                {
                    material.color = red;
                }
                

			} 
			else if(Input.GetKeyUp (KeyCode.Mouse0))  
			{
                //When there is no collision and the mouse left button is clicked, order to start the construction

                gameObject.GetComponent<Renderer> ().material.color = transparentColor;

                //GameObject original color with transparency
                int i = 0;
                foreach (Material material in GetComponent<Renderer>().materials)
                {
                    material.CopyPropertiesFromMaterial(originalMaterials[i]);

                    i++;
                }

                Troop t = new Troop(GameController.Instance.getSelectedUnits().units);
                GameController.Instance.buildingConstruction(gameObject.transform.position,t);
                //gameObject.GetComponent<LOSEntity>().IsRevealer = true;
				gameObject.GetComponent<LOSEntity>().enabled = true;
                enabled = false;
				Destroy (this);
				
			} else {

                // Change the gameObject color to transparent color, indicating that it is posible to create the building there
                foreach (Material material in GetComponent<Renderer>().materials)
                {
                    material.color = transparentColor;
                }
                
			}
		}
	
	}
}
