using UnityEngine;
using System.Collections.Generic;

public class Villager : Focusable
{
    [SerializeField]
    private GameObject buildingPrefab;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
    }
    public void DestroyUnit()
    {
        Destroy(gameObject, 2);
        GameController.Instance.Invoke("ClearSelection",2);
    }

    public void CreateBuilding()
    {
        Instantiate(buildingPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }
}