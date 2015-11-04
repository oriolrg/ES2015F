using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Objective : MonoBehaviour 
{
    public Civilization Controller { get; private set; }

    public float tickValue = .001f;
	public float radius = 10;
	public CivilizationValueDictionary representants;
	public CivilizationValueDictionary control;
	private Civilization maximalCivilization = Civilization.Neutral;

	void Start()
	{
		InvokeRepeating ("detectUnits", 0, 3);
        Controller = gameObject.GetComponentOrEnd<Identity>().civilization;
	}

	void FixedUpdate()
	{
		// If we have a maximal representant
		if (maximalCivilization != Civilization.Neutral) 
		{
			Civilization c = maximalCivilization;
			if (control.ContainsKey (c))
            {
				control [c] = Mathf.Min (control [c] + tickValue, 1 + tickValue - control.Where (x => x.Key != c).Sum (x => x.Value));
			} else {

				control.Add (c, tickValue);
			}
			if (controlDistributed ())
            {
				float toSubstract = substractValue ();

                List<Civilization> lostCivilizations = new List<Civilization>();

				for( int i = 0; i < control.Count; i++ )
                {
                    Civilization otherCiv = control.Keys.ToList()[i];
					// Substract control to the rest of the civilizations
					if (otherCiv != c && control.ContainsKey(otherCiv))
                    {
                        control [otherCiv] = Mathf.Max (0, control [otherCiv] - toSubstract);
                        
                        if (control [otherCiv] <= 0)
							lostCivilizations.Add (otherCiv);
					}
				}

                for( int i = 0; i < lostCivilizations.Count; i++ )
                {
                    control.Remove(lostCivilizations[i]);
                }

                if (control.Count == 1)
                {
                    // new controller
                    Controller = maximalCivilization;
                    GameController.Instance.checkMapControl();
                }
			}
			GameController.Instance.updateControl (gameObject);
		}
	}
	void detectUnits () 
	{
		representants.Clear ();
		Collider[] colliders = Physics.OverlapSphere (transform.position, radius);

		foreach (Collider collider in colliders) 
		{
            if (collider.gameObject == gameObject) continue;

			Identity identity = collider.GetComponent<Identity>();
			if( identity != null )
			{
				Civilization civ = identity.civilization;

                if (civ == Civilization.Neutral) continue;

				if( representants.ContainsKey( civ ) )
				{
					representants[civ]++;
				}
				else
				{
					representants.Add(civ,1);
				}
			}
		}

		maximalCivilization = maximalRepresentant ();


		

	}

	private Civilization maximalRepresentant()
	{
		if (representants.Count == 0)
			return Civilization.Neutral;
		Civilization c = representants.OrderByDescending(x => x.Value).FirstOrDefault().Key;
		float maxValue = representants [c];
		if(representants.Where(x => x.Value == maxValue ).Count () != 1)
			return Civilization.Neutral;
		return c;
	}

	private bool controlDistributed()
	{
		return control.Sum (x => x.Value) >= 1;
	}

	private float substractValue()
	{
		return tickValue / (control.Count-1);
	}


}
 