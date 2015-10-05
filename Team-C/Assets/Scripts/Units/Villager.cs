using UnityEngine;
using System.Collections.Generic;

public class Villager : Focusable
{
    [SerializeField] private GameObject buildingPrefab;
	[SerializeField] private AnimationClip deathAnimation;

    //Flag to add a newly created unit to the list of allied units. Will be true after it's
    //added after the first frame.
    private bool isAdded = false;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
        //GameController.Instance.addAllyUnit(this.gameObject);
    }

    void Update()
    {
        if (!isAdded && this.gameObject.tag == "Ally")
        {
            GameController.Instance.addAllyUnit(this.gameObject);
            isAdded = true;
        }
    }

    public void DestroyUnit()
    {
        GameController.Instance.removeAllyUnit(gameObject);
		GetComponent<Animator> ().Play (deathAnimation.name);
        Destroy(gameObject, deathAnimation.averageDuration);
		GameController.Instance.ClearSelection ();
    }

    public void CreateBuilding()
    {
		GameController.Instance.createBuilding (buildingPrefab);
    }
}