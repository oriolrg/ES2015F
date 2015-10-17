using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public float minFov = 15f;
	public float maxFov = 90f;
	public float sensitivity = 10f;
	
	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		float fov = cam.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov); // make sure fov is in [minFov, maxFov]
		cam.fieldOfView = fov;
	}
}
