using UnityEngine;
using System.Collections;

public class OpenAndClose : MonoBehaviour
{
    private Animator animator;
    private Collider myCollider;
    private bool open = false;
    [SerializeField] float detectionRadius;

    void Start ()
    {
        animator = gameObject.GetComponentOrEnd<Animator>();
        myCollider = gameObject.GetComponentOrEnd<BoxCollider>();  
	}

    void FixedUpdate()
    {
        bool unitsNearBy = detectUnitsNearby();

        if (open && !unitsNearBy)
        {
            open = false;
            animator.SetBool("open", open);
            myCollider.enabled = true;

        }
        if(!open && unitsNearBy)
        {
            open = true;
            animator.SetBool("open", open);
            Invoke("toggleCollider", 3.5f);
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

    private void toggleCollider()
    {
        myCollider.enabled = false;
    }
}
