using UnityEngine;
using System.Collections;

public class OpenAndClose : MonoBehaviour
{
    private Animator animator;
    private bool open = false;
    
	void Start ()
    {
        animator = gameObject.GetComponentOrEnd<Animator>();    

        
	}
	
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.O))
        {
            open = !open;
            animator.SetBool("open", open);
        }
	}
}
