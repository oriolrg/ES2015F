using UnityEngine;
using System.Collections.Generic;

public class TownCenter : StaticUnit 
{
    [SerializeField] private GameObject civilianPrefab;
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject archerPrefab;
    

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateCivilian, CreateSoldier, CreateKnight, CreateArcher, Repair, Sacrifice };
    }

    void Start()
    {
        RallyPoint = transform.position + 5*transform.up;
        Random.seed = Random.seed*2;
        name = string.Format("The {0} Town center", greekAdjectives[Random.Range(0,greekAdjectives.Count)]); 
    }

    public void CreateCivilian()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameObject civilian = (GameObject) GameController.Instance.CreateUnit(transform, civilianPrefab, RallyPoint);
            AI.Instance.assignCivilian(civilian);
        }
    }

    public void CreateSoldier()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, soldierPrefab, RallyPoint);
        }
    }

    public void CreateKnight()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, knightPrefab, RallyPoint);
        }
    }

    public void CreateArcher()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.CreateUnit(transform, archerPrefab, RallyPoint);
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
    
}
