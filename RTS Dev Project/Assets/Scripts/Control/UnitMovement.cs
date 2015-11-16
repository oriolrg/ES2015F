using UnityEngine;
using System.Collections;
using Pathfinding; 

public enum Status{idle, attacking, running, collecting};

public class UnitMovement : MonoBehaviour {

	public Transform target;

	public float repathRate = 0.5f;
	private float lastRepath = -9999;

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
        enabled = true;
	}



	
	public void OnPathComplete(Path p) {
		path = p;
		currentWaypoint = 0;
	}


	void FixedUpdate(){
		if (hasTarget) {

			if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
				lastRepath = Time.time+ Random.value*repathRate*0.5f;
				seeker.StartPath (transform.position,target.position, OnPathComplete);
			}
			/*
			if (Vector3.Distance (targetPos, target.position) > 0) {
				targetPos = target.position;
				seeker.StartPath (transform.position, target.position, OnPathComplete);
			}
			*/
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

			Vector3 v = new Vector3(1.0f,transform.localScale.y/2.0f,1.0f);
			dis = (Vector3.Distance(transform.position,Vector3.Scale(targetPos,v)));

			if(dis < .5){
				timerDeath timer = target.GetComponent<timerDeath>();
				if(timer != null){
					timer.UnitLostTarget(gameObject);
				}
				hasTarget = false;
				var animator = GetComponent<Animator>();
				if (animator != null) animator.SetBool("walk", false);
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
