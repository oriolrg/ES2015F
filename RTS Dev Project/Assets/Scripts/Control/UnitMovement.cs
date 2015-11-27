using UnityEngine;
using System.Collections;
using Pathfinding; 

public enum Status{idle, attacking, running, collecting};

public class UnitMovement : MonoBehaviour {

	public Transform target;

	public float repathRate = 0.5f;
	private float lastRepath = -9999;
	private float nextWaypointDistance = 3;

	[SerializeField] private float speed = 10;

	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	Vector3 targetPos;
    Animator animator;
	AttackController attack;

	public float dis;

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
        CollectResources collect = gameObject.GetComponent<CollectResources>();
        //if (!AI.Instance.resources.Contains(target.tag) & collect != null) if (collect.goingToCollect) collect.goingToCollect = false;
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
		} else {
			p.Release (this);
			Debug.Log ("Oh noes, the target was not reachable: "+p.errorLog);
		}
		
		//seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}


	void FixedUpdate(){
		if (hasTarget) {

			if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
				lastRepath = Time.time+ Random.value*repathRate*0.5f;
				seeker.StartPath (transform.position,target.position, OnPathComplete);
			}
			
			if (path == null) {
				//We have no path to move after yet
				return;
			}
			
			if (currentWaypoint > path.vectorPath.Count) return; 
			if (currentWaypoint == path.vectorPath.Count) {
				currentWaypoint++;
				return;
			}
			
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			dir *= speed;// * Time.deltaTime;
			//transform.Translate (dir);
			characterController.SimpleMove (dir);
			
			//if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			if ( (transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
				currentWaypoint++;
			}


			if(status != Status.attacking){
				Vector3 v = new Vector3(1.0f,transform.localScale.y/2.0f,1.0f);
				dis = (Vector3.Distance(transform.position,Vector3.Scale(targetPos,v)));

                Construct construct = GetComponent<Construct>();
                if (construct != null)
                {
                    if (!construct.getConstruct())
                    {
                        if (targetReached( dis ))
                        {


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
                    }
                }
                else {
                    if (targetReached( dis ))
                    {
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
                    }
                }
            }
        }
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

            if (targetDiagonal == 0) myDiagonal = 0.1f;

            return distanceToTarget < targetDiagonal + myDiagonal;
        }
        else
        {
            Debug.LogError("No collider found in " + target.name + " or in " + name);
        }

        return false;
    }
}
