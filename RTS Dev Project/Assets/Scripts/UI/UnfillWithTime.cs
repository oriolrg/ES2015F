using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnfillWithTime : MonoBehaviour {

	public int time = 5;
	public float remaining = 5;
    public Action callback;

	// Use this for initialization
	void OnEnable () 
	{
		remaining = time;
		Invoke ("Spawn", time);	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (remaining > 0) {
			remaining -= Time.deltaTime;
			float percent = remaining / (float)time;
			GetComponent<Image> ().fillAmount = percent;
			print (Time.deltaTime);
		}
        else
        {
            callback();
            Destroy(gameObject);
        }
	}
}
