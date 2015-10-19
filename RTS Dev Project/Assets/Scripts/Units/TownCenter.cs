using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Focusable 
{
    [SerializeField]
    private GameObject villagerPrefab;

    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
        ini();
        //GameController.Instance.addAllyUnit(gameObject);
        AI.Instance.addCPUUnit(gameObject);
        
    }
    public void CreateUnit()
    {
       GameObject villager = (GameObject) Instantiate(villagerPrefab, transform.position + transform.up * 15, Quaternion.identity);
       AI.Instance.assignVillager(villager);
    }

    void DestroyBuilding()
    {
        GameController.Instance.removeAllyUnit(gameObject);
        GetComponent<Animator>().SetBool("dead", true);
        Destroy(gameObject, 3);
        GameController.Instance.ClearSelection();
        
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
