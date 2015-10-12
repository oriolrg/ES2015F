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
        if (!inConstruction)
        {
            Instantiate(villagerPrefab, transform.position + transform.forward * 3, Quaternion.identity);
        }
        
    }

    void DestroyBuilding()
    {
        if (!inConstruction)
        {
            Destroy(gameObject, 3);

            GameController.Instance.Invoke("ClearSelection", 3);
        }
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
