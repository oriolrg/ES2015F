using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void Action();

public class IngameHUD : MonoBehaviour
{
    [SerializeField] private GameObject actionPanel;

    [SerializeField] private GameObject actionButton;

    private List<Action> actions;

    public void setActions( List<Action> actions)
    {
        this.actions = actions;
    }

    public void refresh()
    {
        foreach( Action action in actions )
        {
            GameObject actionGO = Instantiate(actionButton) as GameObject;
            actionGO.transform.SetParent(actionPanel.transform);
            Action act = action;
            actionGO.GetComponent<Button>().onClick.AddListener(() => { act(); } );
            actionGO.GetComponentInChildren<Text>().text = action.ToString();
            action();

        }
    }

    public void MyMethod() { print(5); }



}
