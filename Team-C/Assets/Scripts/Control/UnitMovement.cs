﻿using UnityEngine;
using System.Collections;
using Pathfinding; 

public class UnitMovement : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] private float speed = 10;

	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	Vector3 targetPos;

	bool hasTarget;

	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		//target = (Transform)GameObject.Find("target").transform;
		//seeker.StartPath(transform.position,target.position,OnPathComplete);
		hasTarget = false;
	}

	public void startMoving( GameObject target )
	{
		this.target = target.transform;
		seeker.StartPath(transform.position,target.transform.position,OnPathComplete);
		targetPos = target.transform.position;
		hasTarget = true;
	}
	

	public void OnPathComplete(Path p) {
		path = p;
		currentWaypoint = 0;
	}


	void FixedUpdate(){

		if (hasTarget) {
			if (Vector3.Distance (targetPos, target.position) > 0) {
				targetPos = target.position;
				seeker.StartPath (transform.position, target.position, OnPathComplete);
			}

			if (path == null) {
				return;
			}
			if (currentWaypoint >= path.vectorPath.Count) {
				return;
			}

			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized * speed;
			characterController.SimpleMove (dir);

			if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < 1.5f) {
				currentWaypoint++;
			}
			if((Vector3.Distance(transform.position,targetPos) < 2)){
				target.GetComponent<timerDeath>().UnitLostTarget(gameObject);
				hasTarget = false;
			}
		}
	}
}
