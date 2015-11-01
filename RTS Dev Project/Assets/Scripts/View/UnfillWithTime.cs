using System;
using UnityEngine;
using UnityEngine.UI;

public class UnfillWithTime : MonoBehaviour
{

    public Action action;

    
	void Start()
    {
        GetComponent<Image>().fillAmount = action.TimeRatio;
    }

    void FixedUpdate ()
    {
        
        if (action.TimeRatio <= 0)
            Destroy(transform.parent.parent.gameObject);
            
        GetComponent<Image>().fillAmount = action.TimeRatio;
               
    }
}
