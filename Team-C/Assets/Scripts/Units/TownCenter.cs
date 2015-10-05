using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Focusable 
{
    [SerializeField] private GameObject villagerPrefab;

    //Flag to add a newly created unit to the list of allied units. Will be true after it's
    //added after the first frame.
    private bool isAdded = false;

    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
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

    public void CreateUnit()
    {
        Instantiate(villagerPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }

    void DestroyBuilding()
    {
        GameController.Instance.removeUnit(gameObject);
        Destroy(gameObject, 3);
        GameController.Instance.Invoke("ClearSelection",3);
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;

        //TEST: TO BE DELETED
        GameController.Instance.killAllEnemies();
    }
}
