using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {

	[SerializeField] private GameObject ground;
	private Camera minimapCamera;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private LayerMask minimapLookAtMask = -1;

	[SerializeField] private float edgeSize = 0.3f;

	private CameraController mainCameraController;
	
	private static float ScreenAspect;

	private Vector3 mainCameraOffset;
	private Vector3 currentLookAtPoint;
	private bool mouseClicked;

	[SerializeField] private RectTransform minimapPanelRectTransform;

	// Use this for initialization
	void Start () {

        mainCamera = Camera.main;
        //mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
        mainCameraController = mainCamera.gameObject.GetComponent<CameraController> ();

		minimapCamera = GetComponent<Camera> ();
		mouseClicked = false;

		updateViewport(((float) Screen.width) / Screen.height); // pass it screen aspect
		updateCameraAttributes ();

		minimapCamera.enabled = true; // show minimap

		// Get mainCameraOffset - vector from point in the ground where camera is looking and camera position
		Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (
			ray, out hit,
			Mathf.Infinity, // max distance
			minimapLookAtMask.value
		)
		    ) {
			mainCameraOffset = mainCamera.transform.position - hit.point;
			currentLookAtPoint = hit.point;
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

		if (!minimapCamera.enabled) {
			return; // Don't allow to interact with the minimap if it isn't enabled
		}

		// Move mainCamera if minimapCamera is clicked or was clicked and now is dragged
		if (Input.GetMouseButtonDown (0)) { // is left-button clicked?
			// Mouse on minimap?
			Vector3 mouse = Input.mousePosition; // get mouse position

			// Normalize mouse coordinates
			mouse.x /= Screen.width;
			mouse.y /= Screen.height;

			mouseClicked = minimapCamera.rect.Contains (mouse); // if rect contains, click was made on minimap
		} else if (Input.GetMouseButton (0)) { // is left-button held down?
			// Held down; keep state
		} else if (Input.GetMouseButtonUp (0)) { // is left-button left?
			mouseClicked = false; // stopped dragging
		} else { // no click?
			mouseClicked = false; // no clicking nor dragging
		}

		// Only move when "clicked"
		if (mouseClicked) {
			// Change position of mainCamera.

			Vector3 position = Input.mousePosition;
			position.x /= Screen.width;
			position.y /= Screen.height;

			if (position.x < minimapCamera.rect.xMin)
				position.x = minimapCamera.rect.xMin;
			else if (position.x > minimapCamera.rect.xMax)
				position.x = minimapCamera.rect.xMax;

			if (position.y < minimapCamera.rect.yMin)
				position.y = minimapCamera.rect.yMin;
			else if (position.y > minimapCamera.rect.yMax)
				position.y = minimapCamera.rect.yMax;
			
			position.x *= Screen.width;
			position.y *= Screen.height;

			// Get position of where we clicked on the minimap
			Ray ray = minimapCamera.ScreenPointToRay (position);
			RaycastHit hit;

			// Where is that point in the ground?
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, // max distance
			                     minimapLookAtMask.value)) {
				//Debug.DrawLine (ray.origin, hit.point);

				// Change camera position, adding offset
				mainCameraController.goTo (hit.point + mainCameraOffset);

				// Set currentLookAtPoint so we can upload it in OnGUI
				currentLookAtPoint = hit.point;
			}
		}
	}

	public void mainCameraTransformUpdate(){
		// Get position of where we clicked on the minimap
		Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward);
		RaycastHit hit;
		
		// Where is that point in the ground?
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, // max distance
		                     minimapLookAtMask.value)) {
			Debug.DrawLine (ray.origin, hit.point);
			
			// Set currentLookAtPoint so we can upload it in OnGUI
			currentLookAtPoint = hit.point;
		}
	}

	private void updateCameraAttributes(){
		// Updates orthographicSize so minimap can display whole ground
		// Based on answer in http://answers.unity3d.com/questions/185141/ortographic-camera-show-all-of-the-object.html

		Bounds bounds = ground.GetComponent<MeshRenderer> ().bounds;
		float boundsAspectRatio = bounds.extents.x / bounds.extents.z;
		float orthographicSize;

		if (boundsAspectRatio < 1) {
			orthographicSize = bounds.extents.z;
		} else {
			orthographicSize = bounds.extents.x / 1;
		}

		minimapCamera.orthographicSize = orthographicSize;// * 1.15f; // also add extra size to display objects at border

//		Vector3 bounds = ground.GetComponent<MeshRenderer> ().bounds.size / 2f;
//		print (bounds.x);
//		print (bounds.z);
//		float boundsAspectRatio = bounds.x / bounds.z;
//		float orthographicSize;
//		
//		if (boundsAspectRatio < 1) {
//			orthographicSize = bounds.z;
//		} else {
//			orthographicSize = bounds.x / 1;
//		}
//		
//		minimapCamera.orthographicSize = orthographicSize;// * 1.15f; // also add extra size to display objects at border
	}

	private void updateViewport(float aspect){
		ScreenAspect = aspect;
		Rect rect = minimapCamera.rect;
		
		if (ScreenAspect > 1) {
			rect.width = edgeSize / ScreenAspect;
			rect.height = edgeSize;
		} else {
			rect.width = edgeSize;
			rect.height = edgeSize / ScreenAspect;
		}

		rect.x = (minimapPanelRectTransform.rect.width / Screen.width - rect.width) / 2f;
		rect.y = (minimapPanelRectTransform.rect.height / Screen.height - rect.height) / 2f;
		
		minimapCamera.rect = rect;
	}
	
	void OnGUI(){
		//minimapCamera.Render (); // To display on top of GUI

		// Draw a rect in the minimap to show the visible area
		Vector2 position = minimapCamera.WorldToScreenPoint(currentLookAtPoint);
		position.y = Screen.height - position.y;

		Vector2 size = minimapCamera.rect.size;
		float sqSize = Mathf.Min (size.x * Screen.width, size.y * Screen.height);
		sqSize *= mainCamera.fieldOfView / 200;

		size = new Vector2 (sqSize, sqSize);
		position -= size / 2f;

		Rect rect = new Rect(position, size);
		//DrawQuad (rect, colorMinimapRect);
		RectDrawer.DrawScreenRectBorder (rect, 2, Color.white);
	}

	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}
}
