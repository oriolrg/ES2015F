using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Focusable 
{
    [SerializeField] private GameObject villagerPrefab;

    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
        ini();
        GameController.Instance.addUnit(this.gameObject);
    }

    void OnDestroy()
    {
        GameController.Instance.removeUnit(gameObject);
    }

    public void CreateUnit()
    {
        Instantiate(villagerPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }

    void DestroyBuilding()
    {
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
