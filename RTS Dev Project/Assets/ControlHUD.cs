using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlHUD : MonoBehaviour 
{
	public float control = 0.5f;
	public float tickValue = .001f;
	public int multiplier = 1;
	public Image positive;
	public Image negative;
	public Text text;

	//test
	void Start()
	{
		Invoke ("reappear", 10);
	}

	public void setMultiplier( int multiplier )
	{
		this.multiplier = multiplier;
		if (multiplier != 0) {
			gameObject.SetActive (true);
		}
	}


	void FixedUpdate () 
	{
		if( multiplier > 0 ) 
		{
			if( control < 1 )
			{
				control += tickValue * multiplier;
				positive.fillAmount = control;
				negative.fillAmount = 1 - control;
			}
			if( control >= 1 )
			{
				text.text = "Controlled";
				Invoke ("Disappear",3);
			}
		}
		if( multiplier < 0 ) 
		{
			if( control > 0 )
			{
				control += tickValue * multiplier;
				positive.fillAmount = control;
				negative.fillAmount = 1 - control;
			}
			if( control <= 1 )
			{
				Invoke ("Disappear",3);
			}
		}
				
	}

	public void Disappear()
	{
		gameObject.SetActive(false);
	}

	public void reappear()
	{
		gameObject.SetActive (true);
	}
}
