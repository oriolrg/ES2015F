using UnityEngine;
using System.Collections.Generic;

public class Archer : Focusable
{
    [SerializeField]
    private GameObject buildingPrefab;

    void Start()
    {
        actions = new List<Command>() { CreateBuilding, DestroyUnit };
        ini();
        //GameController.Instance.addAllyUnit(gameObject);
    }

    public void DestroyUnit()
    {
        GameController.Instance.removeAllyUnit(gameObject);
        Destroy(gameObject, 2);
        GameController.Instance.Invoke("ClearSelection", 2);
    }

    public void CreateBuilding()
    {
        Instantiate(buildingPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }
}