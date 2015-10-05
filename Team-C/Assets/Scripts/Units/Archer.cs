using UnityEngine;
using System.Collections.Generic;

public class Archer : Focusable
{
    [SerializeField]
    private GameObject buildingPrefab;

    //Flag to add a newly created unit to the list of allied units. Will be true after it's
    //added after the first frame.
    private bool isAdded = false;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
        ini();
    }

    void Update()
    {
        if (!isAdded)
        {
            GameController.Instance.addUnit(this.gameObject);
            isAdded = true;
        }
    }

    public void DestroyUnit()
    {
        GameController.Instance.removeUnit(gameObject);
        Destroy(gameObject, 2);
        GameController.Instance.Invoke("ClearSelection", 2);
    }

    public void CreateBuilding()
    {
        Instantiate(buildingPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }
}