using UnityEngine;
using System.Collections;
using Pathfinding; 

public class playercontroller : MonoBehaviour {

	public Transform target;
	public float speed = 10;

	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	Vector3 targetPos;
	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		seeker.StartPath(transform.position,target.position,OnPathComplete);

		targetPos = target.position;
	}
	

	public void OnPathComplete(Path p) {
		path = p;
		currentWaypoint = 0;
	}


	void FixedUpdate(){

		if(Vector3.Distance(targetPos,target.position) > 0){
			targetPos = target.position;
			seeker.StartPath(transform.position,target.position,OnPathComplete);
		}

		if(path == null){
			return;
		}
		if(currentWaypoint >= path.vectorPath.Count){
			return;
		}

		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized * speed;
		characterController.SimpleMove(dir);

		if (Vector3.Distance(transform.position,path.vectorPath[currentWaypoint]) < 2f){
			currentWaypoint++;
		}
	}
}
