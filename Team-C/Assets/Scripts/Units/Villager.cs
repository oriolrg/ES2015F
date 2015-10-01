﻿using UnityEngine;
using System.Collections.Generic;

public class Villager : ShowHUDOnClick 
{
    [SerializeField]
    private GameObject buildingPrefab;

    void Start()
    {
        actions = new List<Action>() { CreateBuilding, DestroyUnit };
    }
    public void DestroyUnit()
    {
        Destroy(gameObject, 2);
    }

    public void CreateBuilding()
    {
        Instantiate(buildingPrefab, transform.position + transform.forward * 3, Quaternion.identity);
    }
}