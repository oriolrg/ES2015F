using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	[SerializeField] float maxMarginDelta = 40; // Pixels. The width border at the edge in which the movement work
	[SerializeField] float minMarginDelta = 1; // Pixels. The width border at the edge in which the movement work
    
    public float speed = 5; // Scale. Speed of the movement

    private Vector3 mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
    private Vector3 mLeftDirection = Vector3.left; // Direction the camera should move when on the left edge
    private Vector3 mUpDirection = Vector3.forward; // Direction the camera should move when on the up edge
    private Vector3 mDownDirection = Vector3.back; // Direction the camera should move when on the down edge

	[SerializeField] private Camera minimapCamera;
    //public float smoothTime = 0.005f; //Controls the velocity of the movement
    //public float deltaMovement = 0.1f; //Error margin for the movement final position

    void Start(){
        
        //goTo(2,-8);
    }


    void Update(){
		bool movement = false;

		if ( (Input.mousePosition.x >= Screen.width - maxMarginDelta && Input.mousePosition.x <= Screen.width - minMarginDelta) || (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) )
        {
            // Move the camera to the right
            transform.position += mRightDirection * Time.deltaTime * speed;
			movement = true;
        }


		if ( (Input.mousePosition.x <= maxMarginDelta && Input.mousePosition.x >= minMarginDelta) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)) )
        {
            // Move the camera to the left
			transform.position += mLeftDirection * Time.deltaTime * speed;
			movement = true;
        }


		if ( (Input.mousePosition.y >= Screen.height - maxMarginDelta && Input.mousePosition.y <= Screen.height - minMarginDelta) || (Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)) )
        {
            // Move the camera up
			transform.position += mUpDirection * Time.deltaTime * speed;
			movement = true;
        }


		if ( (Input.mousePosition.y <= maxMarginDelta && Input.mousePosition.y >= minMarginDelta) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)) )
        {
            // Move the camera down
			transform.position += mDownDirection * Time.deltaTime * speed;
			movement = true;
        }

		if (movement)
			minimapCamera.GetComponent<MinimapCamera>().mainCameraTransformUpdate();
    }


    //Move the camera to the given position 
    public void goTo(float x, float z){

        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        transform.position = newPosition;

		minimapCamera.GetComponent<MinimapCamera>().mainCameraTransformUpdate();

        // StartCoroutine(SmoothMovement(newPosition));

    }

    public void goTo(Vector3 position) { goTo(position.x, position.z); }



    /*
    //Smooth movement of the camera to the position clicked in the minimap. 
    IEnumerator SmoothMovement(Vector3 newPosition)
    {

        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime * smoothTime;
            transform.position = Vector3.Lerp(transform.position, newPosition, timeSinceStarted);

            // If the object has arrived to newPosition with some margin(deltaMovement), stop the coroutine
            if ((Mathf.Abs(transform.position.x - newPosition.x) <= deltaMovement) && (Mathf.Abs(transform.position.z - newPosition.z) <= deltaMovement))
            {

                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }
    */

    
}
