using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	[SerializeField] float maxMarginDelta = 40; // Pixels. The width border at the edge in which the movement work
	[SerializeField] float minMarginDelta = 1; // Pixels. The width border at the edge in which the movement work

    [SerializeField] float speedH = 25; // Scale. Speed of the movement
    [SerializeField] float speedV = 30;

    private Vector3 mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
    private Vector3 mLeftDirection = Vector3.left; // Direction the camera should move when on the left edge
    private Vector3 mUpDirection = Vector3.forward; // Direction the camera should move when on the up edge
    private Vector3 mDownDirection = Vector3.back; // Direction the camera should move when on the down edge

	[SerializeField] private Camera minimapCamera;
    //public float smoothTime = 0.005f; //Controls the velocity of the movement
    //public float deltaMovement = 0.1f; //Error margin for the movement final position


    private Vector3 size;
    private Vector3 origin;


    void Start(){

        //goTo(2,-8);
        //size = GameObject.FindGameObjectWithTag("prova").GetComponent<Renderer>().bounds.size;
        //origin = GameObject.FindGameObjectWithTag("prova").transform.position;
        
    }


    void Update(){

        RaycastHit hit;
        RaycastHit[] hits;
        bool movement = false;
        Vector3 n;
        bool found = false;


        if ( (Input.mousePosition.x >= Screen.width - maxMarginDelta && Input.mousePosition.x <= Screen.width - minMarginDelta) || (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) )
        {
            //Vector3 auxpos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
            n = new Vector3(transform.right.x, 0, transform.right.z);
            transform.Translate(n * speedH * Time.deltaTime, Space.World);
            movement = true;
            //Debug.Log("1pos" + auxpos);
            //Debug.Log("2transform"+transform.position);

            //transform.Translate(-n * speed * Time.deltaTime, Space.World);
            //Debug.Log("3transform reverse"+transform.position);


            
            hits = Physics.RaycastAll(transform.position, transform.forward);

            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.collider.gameObject.tag == "prova")
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            //if (!(Physics.Raycast(transform.position, transform.forward, out hit)))
            {
                //Debug.Log("Entrooooooooooooooooooo"+hit.collider.name);
                //Debug.Log("origin"+hit.point);
                //Debug.Log("if" + hit.point.x +"," origin.x + size.x / 2f + 4f);
                //if ((hit.point.x < origin.x + size.x / 2f + 4f))
                //{

                //transform.position += mRightDirection * Time.deltaTime * speed;
                //transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

                /*
                    n = new Vector3(transform.right.x, 0, transform.right.z);
                    transform.Translate(n * speed * Time.deltaTime, Space.World);
                    movement = true;*/
                //}
                transform.Translate(-n * speedH * Time.deltaTime, Space.World);
                movement = false;
            }
           

                // Move the camera to the right
                //transform.position += mRightDirection * Time.deltaTime * speed;
		        //movement = true;
        }


        if ((Input.mousePosition.x <= maxMarginDelta && Input.mousePosition.x >= minMarginDelta) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
        {
            n = new Vector3(transform.right.x, 0, transform.right.z);
            transform.Translate(-n * speedH * Time.deltaTime, Space.World);
            movement = true;


            hits = Physics.RaycastAll(transform.position, transform.forward);

            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.collider.gameObject.tag == "prova")
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            //if (!(Physics.Raycast(transform.position, transform.forward, out hit)))
            {
                /*if ((hit.point.x > origin.x - size.x / 2f - 4f))
                {

                    //transform.position += mLeftDirection * Time.deltaTime * speed;
                    //transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                    n = new Vector3(transform.right.x, 0, transform.right.z);
                    transform.Translate(-n * speed * Time.deltaTime, Space.World);
                    movement = true;
                }*/

                // Move the camera to the left
                //transform.position += mLeftDirection * Time.deltaTime * speed;
                //movement = true;
                transform.Translate(n * speedH * Time.deltaTime, Space.World);
                movement = false;
            }
        }


        if ((Input.mousePosition.y >= Screen.height - maxMarginDelta && Input.mousePosition.y <= Screen.height - minMarginDelta) || (Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
        {
            n = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.Translate(n * speedV * Time.deltaTime, Space.World);
            movement = true;

            hits = Physics.RaycastAll(transform.position, transform.forward);

            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.collider.gameObject.tag == "prova")
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            // Move the camera up
            //if (!(Physics.Raycast(transform.position, transform.forward, out hit)))
            {
                /*Debug.Log(hit.point);
                if ((hit.point.z < origin.z + size.z / 2f + 4f))
                {

                    //transform.position += mUpDirection * Time.deltaTime * speed;
                    //transform.Translate(new Vector3(0, speed * Time.deltaTime, speed * Time.deltaTime));
                    n = new Vector3(transform.forward.x, 0, transform.forward.z);
                    transform.Translate(n * speed * Time.deltaTime, Space.World);
                    movement = true;
                }*/

                //transform.position += mUpDirection * Time.deltaTime * speed;
                //movement = true;
                
                transform.Translate(-n * speedV * Time.deltaTime, Space.World);
                movement = false;

            }
        }


        if ((Input.mousePosition.y <= maxMarginDelta && Input.mousePosition.y >= minMarginDelta) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
        {

            // Move the camera down
            n = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.Translate(-n * speedV * Time.deltaTime, Space.World);
            movement = true;

            
            hits = Physics.RaycastAll(transform.position, transform.forward);
                
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.collider.gameObject.tag == "prova")
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            //if (!(Physics.Raycast(transform.position, transform.forward, out hit)))
            {
                /*if ((hit.point.z > origin.z - size.z / 2f - 4f))
                {

                    //transform.position += mDownDirection * Time.deltaTime * speed;
                    //transform.Translate(new Vector3(0, -speed * Time.deltaTime, -speed * Time.deltaTime));
                    n = new Vector3(transform.forward.x, 0, transform.forward.z);
                    transform.Translate(-n * speed * Time.deltaTime, Space.World);
                    movement = true;
                }*/

                //transform.position += mDownDirection * Time.deltaTime * speed;
                //movement = true;
                transform.Translate(n * speedV * Time.deltaTime, Space.World);
                movement = false;
            }
        }

        if (movement) minimapCamera.GetComponent<MinimapCamera>().mainCameraTransformUpdate();
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
