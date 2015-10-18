using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Unit 
{
    [SerializeField] private GameObject civilianPrefab;
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject archerPrefab;
    
    
    private Vector3 rallyPoint;


    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateCivilian, CreateSoldier, CreateKnight, CreateArcher, Repair, Sacrifice };
    }

    void Start()
    {
        rallyPoint = Vector3.zero;
    }

    public void CreateCivilian()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, civilianPrefab, rallyPoint);
        }
    }

    public void CreateSoldier()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, soldierPrefab, rallyPoint);
        }
    }

    public void CreateKnight()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, knightPrefab, rallyPoint);
        }
    }

    public void CreateArcher()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, archerPrefab, rallyPoint);
        }
    }

    void Sacrifice()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.removeUnit(gameObject);
            GetComponent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        }
    }

    void Repair()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            health = data.stats[Stat.Health];
        }
    }

    public void setRallyPoint(Vector3 rally)
    {
        rallyPoint = rally;
    }
}
