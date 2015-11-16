﻿using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    [SerializeField] float speed=100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hits;
        RaycastHit hit;
        Vector3 hitPoint;
        bool found = false;

        if (Input.GetKey(KeyCode.Q))
        {

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                hitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                transform.RotateAround(hitPoint, Vector3.up, speed * Time.deltaTime);

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
                {
                    transform.RotateAround(hitPoint, -Vector3.up, speed * Time.deltaTime);
                }
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                hitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                transform.RotateAround(hitPoint, -Vector3.up, speed * Time.deltaTime);

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
                {

                    transform.RotateAround(hitPoint, Vector3.up, speed * Time.deltaTime);
                }
            }

        }

    }
}