using System;
using UnityEngine;
using UnityEngine.UI;

public class UnfillWithTime : MonoBehaviour {

    public QueuedAction action;

	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    GetComponent<Image> ().fillAmount = action.TimeRatio;

        if (action.TimeRatio <= 0)
            Destroy(transform.parent.parent.gameObject);
    }
}
