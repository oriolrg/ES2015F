using UnityEngine;
using System.Collections;
using Pathfinding; 

public class UnitMovement : MonoBehaviour {

	public Transform target;


	[SerializeField] private float speed = 10;

	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	Vector3 targetPos;
    Animator animator;

    public AnimationClip runAnimation;

	public bool hasTarget;

	// Use this for initialization
	void Awake ()
    {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hasTarget = false;
	}

	public void startMoving( GameObject target )
	{
        CollectResources collect = gameObject.GetComponent<CollectResources>();
        if (!AI.Instance.resources.Contains(target.tag) & collect != null) if (collect.goingToCollect) collect.goingToCollect = false;
		this.target = target.transform;
        if ( seeker != null ) seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        
		targetPos = target.transform.position;
		hasTarget = true;
        
        if (animator != null) animator.SetBool("running", true);
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
