﻿using UnityEngine;
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
        Instantiate(villagerPrefab, transform.position + transform.up * 10, Quaternion.identity);
    }

    void DestroyBuilding()
    {
        GameController.Instance.removeAllyUnit(gameObject);
        Destroy(gameObject, 3);
        GameController.Instance.Invoke("ClearSelection",3);
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
