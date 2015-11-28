using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {

	public GameObject ground;
	private Camera minimapCamera;
	[SerializeField] private LayerMask minimapLookAtMask = -1;

	private CameraController mainCameraController;
	
	private static float ScreenAspect;

	private Vector3 mainCameraOffset;
	private Vector3 currentLookAtPoint;
	private bool mouseClicked;

	[SerializeField] private RectTransform minimapPanelRectTransform;

	void Awake () {
		minimapCamera = GetComponent<Camera> ();
		mouseClicked = false;
	}

	// Use this for initialization
	void Start () {
        mainCameraController = Camera.main.gameObject.GetComponent<CameraController> ();

		updateViewport(((float) Screen.width) / Screen.height); // pass it screen aspect
		updateCameraAttributes ();

		minimapCamera.enabled = true; // show minimap

		// Get mainCameraOffset - vector from point in the ground where camera is looking and camera position
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (
			ray, out hit,
			Mathf.Infinity, // max distance
			minimapLookAtMask.value
		)
		    ) {
			mainCameraOffset = Camera.main.transform.position - hit.point;
			currentLookAtPoint = hit.point;
		} else {
			throw new UnityException("MainCamera isn't looking at ground!");
		}
	}
	
	// Update is called once per frame
	void Update (){
		if (Time.timeScale == 0)
			return; // game paused, don't interact

		Vector3 mouse;

		// Move mainCamera if minimapCamera is clicked or was clicked and now is dragged
		if (Input.GetMouseButtonDown (0)) { // is left-button clicked?
			// Mouse on minimap?
			mouse = Input.mousePosition; // get mouse position

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
			// Change position of maiznCamera.

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

		if (Input.GetMouseButton(1)) {
			// Mouse on minimap?
			mouse = Input.mousePosition; // get mouse position
			
			// Normalize mouse coordinates
			mouse.x /= Screen.width;
			mouse.y /= Screen.height;
			
			if(minimapCamera.rect.Contains (mouse)){ // if rect contains, click was made on minimap
				// Now we need the Input.mousePosition again; use it directly
				// Get position of where we clicked on the minimap
				Ray ray = minimapCamera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				// Where is that point in the ground?
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, // max distance
				                     minimapLookAtMask.value)) {
					//Debug.DrawLine (ray.origin, hit.point);
					
					// Change camera position, adding offset
					GameObject target = Instantiate(GameController.Instance.targetPrefab, hit.point, Quaternion.identity) as GameObject;
					GameController.Instance.moveUnits(target);
				}
			}
		}
	}

	public void mainCameraTransformUpdate(){
		// Get position of where we clicked on the minimap
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		
		// Where is that point in the ground?
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, // max distance
		                     minimapLookAtMask.value)) {
			Debug.DrawLine (ray.origin, hit.point);
			
			// Set currentLookAtPoint so we can upload it in OnGUI
			currentLookAtPoint = hit.point;
		}
	}

	public void updateCameraAttributes(){
		// Updates orthographicSize so minimap can display whole ground
		// Based on answer in http://answers.unity3d.com/questions/185141/ortographic-camera-show-all-of-the-object.html

		Bounds bounds = ground.GetComponent<TerrainCollider> ().bounds;
		float boundsAspectRatio = bounds.extents.x / bounds.extents.z;
		float orthographicSize;

		if (boundsAspectRatio < 1) {
			orthographicSize = bounds.extents.z;
		} else {
			orthographicSize = bounds.extents.x / 1;
		}

		minimapCamera.orthographicSize = orthographicSize;// * 1.15f; // also add extra size to display objects at border

		minimapCamera.transform.position = ground.transform.position + new Vector3(bounds.extents.x, 500, bounds.extents.z);
	}

	private void updateViewport(float aspect){
		ScreenAspect = aspect;
		Rect rect = minimapCamera.rect;

		float edgeSize = ((float) minimapPanelRectTransform.rect.height) / Screen.height;
		//edgeSize = 0.295f;
		if (ScreenAspect > 1) {
			rect.width = edgeSize / ScreenAspect;
			rect.height = edgeSize;
		} else {
			rect.width = edgeSize;
			rect.height = edgeSize / ScreenAspect;
		}

		rect.width *= 0.85f;
		rect.height *= 0.85f;

		rect.x = (minimapPanelRectTransform.rect.width / ((float) Screen.width) - rect.width) / 2f;
		rect.y = (minimapPanelRectTransform.rect.height / ((float) Screen.height) - rect.height) / 2f;
		
		minimapCamera.rect = rect;
	}

	void OnGUI(){
		if (Time.timeScale == 0)
			return; // game paused: don't interact

		// minimapCamera.Render (); // To display on top of GUI; CAUTION! slows down everything

		// Draw a rect in the minimap to show the visible area
		Vector2 position = minimapCamera.WorldToScreenPoint(currentLookAtPoint);
		position.y = Screen.height - position.y;

		Vector2 size = minimapCamera.rect.size;
		size.x *= Screen.width * ScreenAspect;
		size.y *= Screen.height;

		//float sqSize = Mathf.Min (size.x * Screen.width, size.y * Screen.height); // whole MinimapRect
		//sqSize *= Camera.main.transform.localPosition.y / 200f; // reduce it with this
		size *= Camera.main.transform.localPosition.y / 200f; // reduce it with this

		//size = new Vector2 (sqSize, sqSize);
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
