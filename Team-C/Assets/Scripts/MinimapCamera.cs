using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {
	
	private Camera mainCamera;
	private Camera cam;

	private static float ScreenAspect;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		cam = GetComponent<Camera> ();
		updateViewport(((float) Screen.width) / Screen.height); // pass it screen aspect
	}
	
	// Update is called once per frame
	void Update (){
//		float aspect = ((float) Screen.width) / Screen.height;
//
//		if (aspect != ScreenAspect) {
//			updateViewport(aspect);
//		}

		if (Input.GetMouseButtonDown (0)) {
			// Clicked on minimap. Change position of mainCamera.

			// Compute offset of camera; its position - where it's looking at
			Vector3 mainCameraOffset;
			Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward); // where is camera looking at
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				mainCameraOffset = mainCamera.transform.position - hit.point;

				// Get position of where we clicked on the minimap
				ray = cam.ScreenPointToRay (Input.mousePosition);
				
				if (Physics.Raycast (ray, out hit)) {
					Debug.DrawLine (ray.origin, hit.point);

					// Change camera position, adding offset
					mainCamera.transform.position = hit.point + mainCameraOffset;
				}
			}
		}
	}

	private void updateViewport(float aspect){
		ScreenAspect = aspect;
		Rect rect = cam.rect;
		
		if (ScreenAspect > 1) {
			rect.width = 0.4f / ScreenAspect;
			rect.height = 0.4f;
		} else {
			rect.width = 0.4f;
			rect.height = 0.4f / ScreenAspect;
		}
		
		rect.x = 1.0f - rect.width;
		rect.y = 1.0f - rect.height;
		
		cam.rect = rect;
	}
}
