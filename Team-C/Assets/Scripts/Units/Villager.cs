using UnityEngine;
using System.Collections.Generic;

public class Villager : Focusable
{
    [SerializeField] private GameObject buildingPrefab;
	[SerializeField] private AnimationClip deathAnimation;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
        GameController.Instance.addUnit(this.gameObject);
    }

    void OnDestroy()
    {
        GameController.Instance.removeUnit(gameObject);
    }

    public void DestroyUnit()
    {
		GetComponent<Animator> ().Play (deathAnimation.name);
        Destroy(gameObject, deathAnimation.averageDuration);
		GameController.Instance.ClearSelection ();
    }

    public void CreateBuilding()
    {
		GameController.Instance.createBuilding (buildingPrefab);
    }
}