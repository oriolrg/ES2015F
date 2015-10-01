using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections.Generic;

public delegate void Action();

public class IngameHUD : MonoBehaviour
{
    [SerializeField] private Transform actionPanel;

    [SerializeField] private Transform statPanel;

    [SerializeField] private GameObject actionPrefab;

    [SerializeField] private GameObject statPrefab;

    [SerializeField] private Image previewPanel;

    public void refresh( Data data, List<Action> actions )
    {
        // Destroy actual widgets
        foreach (Transform child in actionPanel)
            Destroy(child.gameObject);

        foreach (Transform child in statPanel)
            Destroy(child.gameObject);

        // Set preview image
        previewPanel.sprite = data.preview;

        // Set action buttons
        for ( int i = 0; i < data.actionSprites.Count; i++ )
        {
            GameObject actionGO = Instantiate(actionPrefab) as GameObject;
            actionGO.transform.SetParent( actionPanel );

            Action act = actions[i];

            actionGO.GetComponent<Button>().onClick.AddListener(() => { act(); } );
            actionGO.GetComponent<Image>().sprite = data.actionSprites[i];
        }

        // Set stats
        foreach( KeyValuePair<string, float> entry in data.stats )
        {
            GameObject statGO = Instantiate(statPrefab) as GameObject;
            statGO.transform.SetParent(statPanel);

            statGO.GetComponent<Text>().text = string.Format( "- {0} : {1} ", entry.Key, entry.Value);
        }
    }
}
