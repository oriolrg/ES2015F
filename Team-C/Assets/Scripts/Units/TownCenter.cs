using UnityEngine;
using System.Collections.Generic;

public class TownCenter : ShowHUDOnClick 
{
    [SerializeField]
    private GameObject villagerPrefab;
    
    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
    }
    public void CreateUnit()
    {
        Instantiate(villagerPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }

    void DestroyBuilding()
    {
        Destroy(gameObject, 3);
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
