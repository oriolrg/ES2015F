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
        if (!inConstruction)
        {
            Instantiate(villagerPrefab, transform.position + transform.up * 10, Quaternion.identity);
        }
    }

    void DestroyBuilding()
    {
        if (!inConstruction)
        {
            GameController.Instance.removeAllyUnit(gameObject);
            GetComponent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
            GameController.Instance.ClearSelection();
        }
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    
}
