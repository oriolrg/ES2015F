using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Unit 
{
    [SerializeField] private GameObject villagerPrefab;
    private Vector3 rallyPoint;


    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateUnit, DestroyBuilding };
    }

    void Start()
    {
        rallyPoint = transform.position - transform.forward*3;
    }

    public void CreateUnit()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, villagerPrefab, rallyPoint);
        }
    }

    void DestroyBuilding()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.removeUnit(gameObject);
            GetComponent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        }
    }

    void Upgrade()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public void setRallyPoint(Vector3 rally)
    {
        rallyPoint = rally;
    }
}
