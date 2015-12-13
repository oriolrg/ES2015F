using UnityEngine;
using System.Collections;

public class OpenAndClose : MonoBehaviour
{
    private Animator animator;
    private bool open = false;
    [SerializeField] float detectionRadius;

    void Start ()
    {
        animator = gameObject.GetComponentOrEnd<Animator>();    
	}

    void FixedUpdate()
    {
        bool unitsNearBy = detectUnitsNearby();

        if (open && !unitsNearBy)
        {
            open = false;
            animator.SetBool("open", open);
        }
        if(!open && unitsNearBy)
        {
            open = true;
            animator.SetBool("open", open);
        }
    }

    private bool detectUnitsNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        bool unitsNearBy = false;
        int i = 0;

        while (!unitsNearBy && i < colliders.Length)
        {
            Identity identity = colliders[i].GetComponent<Identity>();

            unitsNearBy = identity != null && !identity.unitType.isBuilding();
            i++;
        }

        return unitsNearBy;
    }
}
