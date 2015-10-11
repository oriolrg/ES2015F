using UnityEngine;
using System.Collections.Generic;

public class Villager : Focusable
{
    [SerializeField] private GameObject buildingPrefab;
	[SerializeField] private AnimationClip deathAnimation;

    [SerializeField] private float dist;

    private GameObject buildingToConstruct;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
        //GameController.Instance.addAllyUnit(gameObject);

        dist = 2f;
    }

    void Update()
    {
        if (buildingToConstruct != null)
        {
            if((transform.position-buildingToConstruct.transform.position).magnitude < dist)
            {
                Debug.Log("A CONSTRUIIIIR!!!!!!!!");
                buildingToConstruct.GetComponent<BuildingConstruction>().startConstruction();
            }
        }
    }


    public void DestroyUnit()
    {
        GameController.Instance.removeAllyUnit(gameObject);
		GetComponentInParent<Animator> ().SetBool("dead", true);
        Destroy(gameObject, 3);
		GameController.Instance.ClearSelection ();
    }

    public void CreateBuilding()
    {
        Debug.Log("Create building VILLAGER");
		GameController.Instance.createBuilding (buildingPrefab);
    }

    public void setBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }


}