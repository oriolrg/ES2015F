using UnityEngine;
using System.Collections;

public class AnimationsLoopSoldat : MonoBehaviour
{
	Animator animator;
	int i = 0;

	void Animations()
	{
		if (i == 1) {
			GetComponent<Animator>().SetBool ("attack", false);
			GetComponent<Animator>().SetBool ("walk", true);
			i++;
		} else if (i == 2) {
			GetComponent<Animator>().SetBool ("walk", false);
			GetComponent<Animator>().SetBool ("attack", true);
			i++;
		} else if (i == 3) {
			GetComponent<Animator>().SetBool ("attack", false);
			GetComponent<Animator>().SetBool ("die", true);
			i++;
		} else if (i == 4)
		{
			i = 0;
		}
		else if (i == 0)
		{
			i++;
		}

	}

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("Animations", 1, 5);	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	} 
}

