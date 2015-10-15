using UnityEngine;
using System.Collections.Generic;

public class Archer : Focusable
{
    [SerializeField]
    private GameObject wonder;

    protected override List<Action> defineActions()
    {
        return new List<Action>() { CreateWonder, DestroyUnit };
    }

    public void CreateWonder()
    {
        GameController.Instance.createBuilding(wonder);
    }
}
