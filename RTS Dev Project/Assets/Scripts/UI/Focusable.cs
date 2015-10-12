using UnityEngine;
using System.Collections.Generic;

public abstract class Focusable : MonoBehaviour
{
    [SerializeField]
    protected UnitData data;

    [SerializeField]
    protected IngameHUD hud;

    [SerializeField]
    protected List<Action> actions;

    protected bool inConstruction;

    protected bool construct;


    void start()
    {
        inConstruction = false;
        construct = false;
    }

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

    public void SetInConstruction(bool b)
    {
        inConstruction = b;Debug.Log(b);
    }

    public void ActivateBuildingToConstruct()
    {
        construct = true;
    }
}
