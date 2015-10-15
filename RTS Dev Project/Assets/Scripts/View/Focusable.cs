using UnityEngine;
using System.Collections.Generic;

public abstract class Focusable : MonoBehaviour
{

    [SerializeField]
    private UnitData data;

    [SerializeField]
    protected List<Action> actions;

    private List<Action> actions;



    private const string lengthMismatchMessage = "The list of actions and the actionSprites dont match in length: {0} vs {1}. GameObject: {2}. Forgot death sprite?";



    public virtual void Start()
    {
        actions = defineActions();

        if (actions.Count != data.actionSprites.Count)
        {
            Debug.LogError(string.Format(lengthMismatchMessage, actions.Count, data.actionSprites.Count, gameObject));
            Destroy(this);
        }

        GameController.Instance.addUnit(gameObject);
    }

    protected abstract List<Action> defineActions();

    public void onFocus()
    {
        //GameController.Instance.RefreshHUD(data, actions);
    }

    void OnDestroy()
    {
        GameController.Instance.removeUnit(gameObject);
    }

    // Shared actions

    public void DestroyUnit()
    {
        GetComponent<Animator>().Play(deathAnimation.name);
        Destroy(gameObject, deathAnimation.averageDuration);
        GameController.Instance.ClearSelection();
    }
}
