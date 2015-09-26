using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private float marginDelta = 10; // Pixels. The width border at the edge in which the movement work
    public float speed = 3; // Scale. Speed of the movement

    private Vector3 mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
    private Vector3 mLeftDirection = Vector3.left;
    private Vector3 mUpDirection = Vector3.forward;
    private Vector3 mDownDirection = Vector3.back;

    void Start(){
        
        //goTo(2,-8);
    }


    void Update(){

        if ( (Input.mousePosition.x >= Screen.width - marginDelta) || (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) )
        {
            // Move the camera to the right
            transform.position += mRightDirection * Time.deltaTime * speed;
        }


        if ( (Input.mousePosition.x <= marginDelta) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)) )
        {
            // Move the camera to the left
            transform.position += mLeftDirection * Time.deltaTime * speed;
        }


        if ( (Input.mousePosition.y >= Screen.height - marginDelta) || (Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)) )
        {
            // Move the camera up
            transform.position += mUpDirection * Time.deltaTime * speed;
        }


        if ( (Input.mousePosition.y <= marginDelta) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)) )
        {
            // Move the camera down
            transform.position += mDownDirection * Time.deltaTime * speed;
        }
    }


    //Move the camera to the given position 
    void goTo(float x, float z){

        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        transform.position = newPosition;

    }
}
