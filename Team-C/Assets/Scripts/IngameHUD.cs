using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void Action();

public class IngameHUD : MonoBehaviour
{
    [SerializeField] private GameObject actionButtonPanel;

    [SerializeField] private GameObject statPanel;

    [SerializeField] private GameObject actionButton;

    [SerializeField] private GameObject statText;

    [SerializeField] private GameObject preview;

    public void refresh( List<Action> actions, Dictionary<string,float> stats, Sprite previewImage )
    {
        // Destroy actual widgets
        foreach (Transform child in actionButtonPanel.transform)
            GameObject.Destroy(child.gameObject);

        foreach (Transform child in statPanel.transform)
            GameObject.Destroy(child.gameObject);

        // Set preview image
        preview.GetComponent<Image>().sprite = previewImage;

        // Set action buttons
        foreach ( Action action in actions )
        {
            GameObject actionGO = Instantiate(actionButton) as GameObject;
            actionGO.transform.SetParent( actionButtonPanel.transform );

            // Store correct reference
            Action _action = action;
            actionGO.GetComponent<Button>().onClick.AddListener(() => { _action(); } );

        }

        // Set stats
        foreach( KeyValuePair<string, float> entry in stats )
        {
            GameObject statGO = Instantiate(statText) as GameObject;
            statGO.transform.SetParent(statPanel.transform);

            statGO.GetComponent<Text>().text = string.Format( "- {0} : {1} ", entry.Key, entry.Value);
        }
    }
}
