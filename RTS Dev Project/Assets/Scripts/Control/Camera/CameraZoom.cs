using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	[SerializeField] float min = 5f;
	[SerializeField] float max = 70f;
	//public float sensitivity = 10f;

    public float speed = 50;
	
	//private Camera cam;

	// Use this for initialization
	void Start () {
		//cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            //transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            if (transform.localPosition.y > min) transform.position += transform.forward * Time.deltaTime * speed;
            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            //transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            if (transform.localPosition.y < max) transform.position += -transform.forward * Time.deltaTime * speed;
        }


        /*float fov = cam.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov); // make sure fov is in [minFov, maxFov]
		cam.fieldOfView = fov;*/
    }
}
