using UnityEngine;
using System.Collections.Generic;

public class Farm : Focusable
{
    [SerializeField]
    private GameObject villagerPrefab;

    void Start()
    {
        actions = new List<Action>() { CreateUnit, DestroyBuilding, Upgrade };
        ini();
    }

    public void CreateUnit()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            Instantiate(villagerPrefab, transform.position + transform.forward * 3, Quaternion.identity);
        }
        
    }

    void DestroyBuilding()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            Destroy(gameObject, 3);

            GameController.Instance.Invoke("ClearSelection", 3);
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
