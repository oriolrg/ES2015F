using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {

    public GameObject prefab;
	[SerializeField] private GameObject provisionalPrefab;


    void Start()
    {

    }


    void Update()
    {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);

		foreach (RaycastHit hit in hits) {

			if (hit.collider.gameObject.tag == "Ground") {
				provisionalPrefab.transform.position = new Vector3 (hit.point.x, prefab.transform.position.y, hit.point.z);

				//When the left button of the mouse is clicked, get the position and create a prefab there.
				if (Input.GetKeyDown (KeyCode.Mouse0)) 
				{
					Color color = provisionalPrefab.GetComponent<Renderer> ().material.color;
					
					provisionalPrefab.GetComponent<Renderer> ().material.color = new Color(color.r, color.g, color.b,1f);

					GetComponent<GameController> ().enabled = true;
					enabled = false;

				}



			}
		}



    }

	void OnEnable()
	{
		provisionalPrefab = Instantiate (prefab, Vector3.zero, prefab.transform.rotation) as GameObject;
		Color color = provisionalPrefab.GetComponent<Renderer> ().material.color;

		provisionalPrefab.GetComponent<Renderer> ().material.color = new Color(color.r, color.g, color.b,0.5f);
	}
}
