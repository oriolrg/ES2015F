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
     
    }

    public void CreateUnit()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            Instantiate(villagerPrefab, transform.position + transform.up * 10, Quaternion.identity);
        }
    }

    void DestroyBuilding()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.removeAllyUnit(gameObject);
            GetComponent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
            GameController.Instance.ClearSelection();
        }
    }

    void Upgrade()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    
}
