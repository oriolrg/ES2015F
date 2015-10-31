using UnityEngine;
using System.Collections.Generic;

public class Stable : StaticUnit 
{
    [SerializeField] private GameObject knightPrefab;
    

    protected override List<Command> defineCommands()
    {
        return new List<Command>() {CreateKnight, Sacrifice, Repair };
    }

    void Start()
    {
        RallyPoint = transform.position + 5*transform.up;
        Random.seed = Random.seed*2;
        name = string.Format("The {0} Town center", greekAdjectives[Random.Range(0,greekAdjectives.Count)]); 
    }

    

    public void CreateKnight()
    {
        //if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        
        GameController.Instance.CreateUnit(transform, knightPrefab, RallyPoint);
        
    }

    void Sacrifice()
    {
        //if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        
        GameController.Instance.removeUnit(gameObject);
        GetComponent<Animator>().SetBool("dead", true);
        Destroy(gameObject, 3);
        
    }

    void Repair()
    {
        //if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        
        health = data.stats[Stat.Health];
    
    }
    
}
