using System;
using UnityEngine;
using UnityEngine.UI;

public class UnfillWithTime : MonoBehaviour {

	public float time = 5;
	public float remaining = 5;
    public Command callback;

	// Use this for initialization
	void OnEnable () 
	{
		remaining = time;
		Invoke ("timeEnded", time);	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (remaining > 0)
        {
			remaining -= Time.deltaTime;
			float percent = remaining / (float)time;
			GetComponent<Image> ().fillAmount = percent;
		}
	}

    public void timeEnded()
    {
        callback();
        
        Destroy(gameObject.transform.parent.parent.gameObject);
        
    }

    void OnMouseDown()
    {
        print("creation stopped");
        Destroy(gameObject.transform.parent);
    }
}
