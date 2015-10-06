using UnityEngine;
using System.Collections.Generic;

public class Infantry : Focusable
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