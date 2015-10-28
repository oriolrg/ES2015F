using UnityEngine;
using System.Collections.Generic;

public class timerDeath : MonoBehaviour
{
	
	public List<GameObject> unitsGoingHere;

	// Use this for initialization
	void Awake () 
	{
		unitsGoingHere = new List<GameObject> ();
	}
	public void AddUnit( GameObject unit )
	{
		unitsGoingHere.Add (unit);
	}

	public void UnitLostTarget( GameObject unit )
	{
		unitsGoingHere.Remove (unit);
		if( unitsGoingHere.Count == 0 )
			Destroy (gameObject);
	}
}
