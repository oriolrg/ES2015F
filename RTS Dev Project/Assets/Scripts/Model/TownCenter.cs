using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Focusable
{
    [SerializeField]
    private GameObject villager;

    protected override List<Action> defineActions()
    {
        return new List<Action>() { CreateVillager, Upgrade, DestroyUnit };
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
