using UnityEngine;
using System.Collections;
using Pathfinding; 

public enum Status{idle, attacking, running, collecting};

public class UnitMovement : MonoBehaviour {

	public Transform target;

	private float repathRate = 0.5f;
	private float lastRepath = -9999;
	private float nextWaypointDistance = 3;

	[SerializeField] private float speed = 10;

	Seeker seeker;
	Path path;
	private int currentWaypoint;
	CharacterController characterController;
	public Vector3 targetPos;
    Animator animator;
	AttackController attack;


	private Vector2 currentPosition;
	private Vector2 posTarget;
	public float distanceToTarget;


	private float currentPathCount;


	public bool hasTarget;

	public Status status;

    public VoidMethod callback;

	// Use this for initialization
	void Awake ()
    {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		attack = GetComponent<AttackController> ();
        animator = GetComponent<Animator>();
        hasTarget = false;
		status = Status.idle;
        callback = null;
	}

	public void startMoving( GameObject target, VoidMethod callback = null )
	{
        CollectResources collecting = GetComponent<CollectResources>();
        if( collecting != null )
            collecting.CancelInvoke();

        this.callback = callback;

		this.target = target.transform;
		if ( seeker != null ) seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        
		targetPos = target.transform.position;
		hasTarget = true;

        // Cancel all animations and play walk
        if (animator != null)
        {
            foreach(AnimatorControllerParameter param in animator.parameters)
            {
                animator.SetBool(param.name, false);
            }
            animator.SetBool("walk", true);
        }
		status = Status.running;
	}

	

	public void OnPathComplete (Path p) {
		p.Claim (this);
		if (!p.error) {
			if (path != null) path.Release (this);
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
			currentPathCount = path.vectorPath.Count;
		} else {
			p.Release (this);
			Debug.Log ("The target was not reachable: "+p.errorLog);
		}
	}


	void FixedUpdate(){

		//If the unit have somewhere to go
		if (hasTarget) {
			Construct construct = GetComponent<Construct>(); //See if the unit has something to cunstruct

			//Recalculate path
			//if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
		    //	lastRepath = Time.time+ Random.value*repathRate*0.5f;
			//	seeker.StartPath (transform.position,targetPos, OnPathComplete);
			//}

			if (path == null) {
				return;
			}

			currentPosition = new Vector2(transform.position.x,transform.position.z);
			posTarget = new Vector2(targetPos.x,targetPos.z);

			//Distance to the target from the current position
			distanceToTarget = Vector2.Distance(currentPosition,posTarget);


			//If the unit hasn't reached the target yet
			if(!targetReached(distanceToTarget)){

				Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized; //direction to move along

				transform.LookAt(transform.position + new Vector3(dir.x,0f,dir.z));
				dir *= speed*Time.deltaTime;
                characterController.Move(dir);

				//If the unit has moved enough go to the next waypoint of the grid
				if ( (transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance && currentWaypoint < currentPathCount - 1) currentWaypoint++;
			} else {
				if(status != Status.attacking){ 
					if (construct != null) {
						if (!construct.getConstruct()){
							stopUnit();
						}

					}
				}
			}

		}
	}
	
	public void stopUnit(){
		timerDeath timer = target.GetComponent<timerDeath>();
		if (timer != null)
		{
			timer.UnitLostTarget(gameObject);
		}
		hasTarget = false;
		var animator = GetComponent<Animator>();
		if (animator != null)
		{
			animator.SetBool("walk", false);
		}
		status = Status.idle;
		// If there is any callback, call it
		if (callback != null)
			callback();
	}

    private bool targetReached( float distanceToTarget )
    {
        BoxCollider targetCollider = target.GetComponent<BoxCollider>();
        BoxCollider myCollider = this.GetComponent<BoxCollider>();

        if( targetCollider != null && myCollider != null )
        {
            Vector3 targetExtents = targetCollider.bounds.extents;
            Vector3 myExtents = myCollider.bounds.extents;

            float targetDiagonal = new Vector2(targetExtents.x, targetExtents.z).magnitude;
            float myDiagonal = new Vector2(myExtents.x, myExtents.z).magnitude;

            if (targetDiagonal == 0) myDiagonal = 0.2f;

            return distanceToTarget < targetDiagonal + myDiagonal;
        }
        else
        {
            Debug.LogError("No collider found in " + target.name + " or in " + name);
        }

        return false;
    }

	
}
