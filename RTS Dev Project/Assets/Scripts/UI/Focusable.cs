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

    protected bool inConstruction; //Indicates if a building is in construction or if a unit is constructing a building

    protected bool construct; //Indicates if a unit has the order to construct a building


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

    public void setConstruct(bool b)
    {
        construct = b;
    }

    public bool getConstruct()
    {
        return construct;
    }

    public bool getInConstruction()
    {
        return inConstruction;
    }
}
