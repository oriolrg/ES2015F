using UnityEngine;
using System.Collections;

public class provarotation : MonoBehaviour {

    public float speed=100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Q))
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Vector3 a = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                transform.RotateAround(a, Vector3.up, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Vector3 a = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                transform.RotateAround(a, -Vector3.up, speed * Time.deltaTime);
            }

        }

    }
}
