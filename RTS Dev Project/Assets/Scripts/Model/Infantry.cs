using UnityEngine;
using System.Collections.Generic;

public class Infantry : Focusable
{
    [SerializeField]
    private GameObject buildingPrefab;

    void Start()
    {
        actions = new List<Command>() { CreateBuilding, DestroyUnit };
        ini();
        Debug.Log("Start Vill");
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