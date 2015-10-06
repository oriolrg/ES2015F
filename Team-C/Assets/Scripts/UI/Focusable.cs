﻿using UnityEngine;
using System.Collections.Generic;

public abstract class Focusable : MonoBehaviour
{
    [SerializeField]
    protected UnitData data;

    [SerializeField]
    protected IngameHUD hud;

    [SerializeField]
    protected List<Action> actions;

    protected void ini()
    {
        hud = GameController.Instance.hud;
        if (actions.Count != data.actionSprites.Count)
            throw new System.Exception("the number of actions and actionSprites differ");
    }

    public void onFocus()
    {
        hud.Refresh(data, actions);
    }
}