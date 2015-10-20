using UnityEngine;
using System.Collections.Generic;

public class Archery : StaticUnit 
{
    [SerializeField] private GameObject archerPrefab;
    

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateArcher,  Sacrifice, Repair };
    }

    void Start()
    {
        RallyPoint = transform.position + 5*transform.up;
        Random.seed = Random.seed*2;
        name = string.Format("The {0} Town center", greekAdjectives[Random.Range(0,greekAdjectives.Count)]); 
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
