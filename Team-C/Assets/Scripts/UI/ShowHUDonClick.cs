using UnityEngine;
using System.Collections.Generic;

public abstract class ShowHUDOnClick : MonoBehaviour
{
    [SerializeField] protected UnitData data;

    [SerializeField] protected IngameHUD hud;

    [SerializeField] protected List<Action> actions;

    void Start()
    {
        if (actions.Count != data.actionSprites.Count)
            throw new System.Exception("the number of actions and actionSprites differ");
    }

    void OnMouseDown()
    {
        hud.refresh( data, actions );
    }
}
