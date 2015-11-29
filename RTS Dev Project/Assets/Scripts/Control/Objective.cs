using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Objective : MonoBehaviour 
{
    public Player Controller { get; private set; }

    public float tickValue = .001f;
	public float radius = 10;
	public PlayerValueDictionary representants;
	public PlayerValueDictionary control;
	private Player maximalPlayer = Player.Neutral;

	void Start()
	{
		InvokeRepeating ("detectUnits", 0, 3);
        Controller = gameObject.GetComponentOrEnd<Identity>().player;
	}

	void FixedUpdate()
	{
		// If we have a maximal representant
		if (maximalPlayer != Player.Neutral) 
		{
			Player c = maximalPlayer;
			if (control.ContainsKey (c))
            {
				control [c] = Mathf.Min (control [c] + tickValue, 1 + tickValue - control.Where (x => x.Key != c).Sum (x => x.Value));
			} else {

				control.Add (c, tickValue);
			}
			if (controlDistributed ())
            {
				float toSubstract = substractValue ();

                List<Player> lostPlayers = new List<Player>();

				for( int i = 0; i < control.Count; i++ )
                {
                    Player otherPlayer = control.Keys.ToList()[i];
					// Substract control to the rest of the civilizations
					if (otherPlayer != c && control.ContainsKey(otherPlayer))
                    {
                        control [otherPlayer] = Mathf.Max (0, control [otherPlayer] - toSubstract);
                        
                        if (control [otherPlayer] <= 0)
							lostPlayers.Add (otherPlayer);
					}
				}

                for( int i = 0; i < lostPlayers.Count; i++ )
                {
                    control.Remove(lostPlayers[i]);
                }

                if (control.Count == 1)
                {
                    // new controller
                    Controller = maximalPlayer;
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
				Player player = identity.player;

                if (player == Player.Neutral) continue;

				if( representants.ContainsKey( player ) )
				{
					representants[player]++;
				}
				else
				{
					representants.Add(player,1);
				}
			}
		}

		maximalPlayer = maximalRepresentant ();


		

	}

	private Player maximalRepresentant()
	{
		if (representants.Count == 0)
			return Player.Neutral;
		Player c = representants.OrderByDescending(x => x.Value).FirstOrDefault().Key;
		float maxValue = representants [c];
		if(representants.Where(x => x.Value == maxValue ).Count () != 1)
			return Player.Neutral;
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
 