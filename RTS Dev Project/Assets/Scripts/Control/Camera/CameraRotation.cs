using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;

    public float damping = 1;

    public bool move;

    void Start()
    {
        RaycastHit hit;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward);
        hit = hits[0];
        for (int i = 0; i < hits.Length; i++)
        {
            hit = hits[i];
            if (hit.collider.gameObject.tag == "Ground") break;
        }
        if (hit.collider.gameObject.tag == "Ground")
        {

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{
            Debug.Log(target.transform.position);
            target.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Debug.Log(target.transform.position);
            transform.LookAt(target.transform);
        }
        //offset = target.transform.position - transform.position;
        offset = new Vector3(0,-9.5f,9.5f);
        Debug.Log("ofset: " + offset);
        move = false;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            target.transform.Rotate(0, 2f, 0);
            move = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            target.transform.Rotate(0, -2f, 0);
            move = true;
        }
    }

    void LateUpdate()
    {
        if (move)
        {
            move = false;
            RaycastHit hit;
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, transform.forward);
            hit = hits[0];
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if(hit.collider.gameObject.tag=="Ground") break;
            }
            if (hit.collider.gameObject.tag == "Ground")
            {


                //if (Physics.Raycast(transform.position, transform.forward, out hit))
                //{
                Debug.Log(hit.collider.gameObject.tag);
                //GameObject target = Instantiate(empty, hit.point, transform.rotation) as GameObject;
                target.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                



                //float currentAngle = transform.eulerAngles.y;
                float desiredAngle = target.transform.eulerAngles.y;
                //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

                Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
                Vector3 aux = target.transform.position - (rotation * offset);
                transform.position = new Vector3(aux.x,transform.position.y,aux.z);
                Debug.Log("ara"+transform.position);
                transform.LookAt(target.transform);
                Debug.Log("despres" + transform.position);
                //transform.position = new Vector3(transform.position.x,20.9f,transform.position.z);


                //}
            }

        }
    }
}
