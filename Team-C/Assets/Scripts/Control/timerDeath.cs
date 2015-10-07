using UnityEngine;
using System.Collections.Generic;

public class timerDeath : MonoBehaviour
{
	private int cnt = 0;
	
	public List<GameObject> unitsGoingHere;

	// Use this for initialization
	void Start () 
	{
		unitsGoingHere = new List<GameObject> ();
		//Destroy (gameObject, timer);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddUnit( GameObject unit )
	{
		unitsGoingHere.Add (unit);
		cnt += 1;
		//print (unitsGoingHere.Count);
	}

	public void AddUnitList( List<GameObject> unit )
	{
		unitsGoingHere.AddRange (unit);
	}

	public void UnitLostTarget( GameObject unit )
	{
		unitsGoingHere.Remove (unit);
		cnt -= 1;
		print (cnt);
		if (cnt == 0)
			Destroy (gameObject);
	}
}
