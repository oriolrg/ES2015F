using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {

	public LayerMask minimapLayerMask = -1;

	private Camera mainCamera;
	private CameraController mainCameraController;
	private Camera cam;

	private Vector3 mainCameraOffset;

	private static float ScreenAspect;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		mainCameraController = mainCamera.gameObject.GetComponent<CameraController> ();

		cam = GetComponent<Camera> ();

		updateViewport(((float) Screen.width) / Screen.height); // pass it screen aspect
		updateCameraAttributes ();

		cam.enabled = true; // show minimap

		// Get mainCameraOffset - vector from point in the ground where camera is looking and camera position
		Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (
			ray, out hit,
			Mathf.Infinity, // max distance
			minimapLayerMask.value
		)
		    ) {
			mainCameraOffset = mainCamera.transform.position - hit.point;
		} else {
			throw new UnityException("MainCamera isn't looking at ground!");
		}
	}
	
	// Update is called once per frame
	void Update (){
//		float aspect = ((float) Screen.width) / Screen.height;
//
//		if (aspect != ScreenAspect) {
//			updateViewport(aspect);
//		}

		if (!cam.enabled) {
			return; // Don't allow to interact with the minimap if it isn't enabled
		}

		// Move mainCamera if minimapCamera is clicked
		if (Input.GetMouseButton (0)){ // is left-button held down? (includes click)

			// Mouse on minimap?

			Vector3 mouse = Input.mousePosition; // get mouse position
			// Normalize mouse coordinates
			mouse.x /= Screen.width;
			mouse.y /= Screen.height;

			if (cam.rect.Contains(mouse)){ // Clicked on minimap. 
				// Change position of mainCamera.

				// Get position of where we clicked on the minimap
				Ray ray = cam.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				// Where is that point in the ground?
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, // max distance
				                     minimapLayerMask.value)) {
					Debug.DrawLine (ray.origin, hit.point);
					
					// Change camera position, adding offset
					mainCameraController.goTo(hit.point + mainCameraOffset);
				}
			}
		}
	}

	private void updateCameraAttributes(){
		// Updates orthographicSize so minimap can display whole ground
		// Based on answer in http://answers.unity3d.com/questions/185141/ortographic-camera-show-all-of-the-object.html
		Bounds bounds = GameObject.Find ("Ground").GetComponent<MeshRenderer> ().bounds;
		float boundsAspectRatio = bounds.extents.x / bounds.extents.z;
		float orthographicSize;

		if (boundsAspectRatio < 1) {
			orthographicSize = bounds.extents.y;
		} else {
			orthographicSize = bounds.extents.x / 1;
		}

		cam.orthographicSize = orthographicSize * 1.15f; // also add extra size to display objects at border
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
