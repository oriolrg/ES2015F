using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Unit
{
    [SerializeField]
    private GameObject villager;

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateVillager, Upgrade };
    }

    public void CreateVillager()
    {
        Instantiate(villager, transform.position + transform.forward * 3, Quaternion.identity);
    }

    void Upgrade()
    {
        GetComponent<Renderer>().material.color = Color.green;

        //TEST: TO BE DELETED
        GameController.Instance.killAllEnemies();
    }
}
