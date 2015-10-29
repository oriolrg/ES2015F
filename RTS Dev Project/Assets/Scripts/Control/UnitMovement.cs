﻿using UnityEngine;
using System.Collections;
using Pathfinding; 


enum Status{idle, attacking, running, collecting};

public class UnitMovement : MonoBehaviour {

	public Transform target;


	[SerializeField] private float speed = 10;

	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	Vector3 targetPos;
    Animator animator;
	AttackController attack;


    public AnimationClip runAnimation;

	public bool hasTarget;

	public Status status;

	// Use this for initialization
	void Awake ()
    {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		attack = GetComponent<AttackController> ();
        animator = GetComponent<Animator>();
        hasTarget = false;
		status = Status.idle;

	}

	public void startMoving( GameObject target )
	{
		this.target = target.transform;
        if ( seeker != null )
        
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        
		targetPos = target.transform.position;
		hasTarget = true;
        
        if (animator != null) animator.SetBool("running", true);

		status = Status.running;
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

            transform.LookAt(path.vectorPath[currentWaypoint]);

			if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < 1.5f) {
				currentWaypoint++;
			}
			if((Vector3.Distance(transform.position,targetPos) < 2)){
				target.GetComponent<timerDeath>().UnitLostTarget(gameObject);
				hasTarget = false;
                var animator = GetComponent<Animator>();
                if (animator != null) animator.SetBool("running", false);
				status = Status.idle;
            }
		}
	}

	/*
	void OnTriggerEnter(Collider collider) {	
		print (gameObject.name + "|" + collider.name);
		if (!collider.gameObject.Equals (gameObject) && collider.gameObject.tag=="Ally"){//collider.gameObject.GetComponent<UnitMovement>() != null){
			print (gameObject.name + "|" + collider.name);
			//gameObject.transform.position += gameObject.transform.right * GetComponent<BoxCollider> ().size.magnitude;
			print ("adios");
		}
			
	}*/
}
