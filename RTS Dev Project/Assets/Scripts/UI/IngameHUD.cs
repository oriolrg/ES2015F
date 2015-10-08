using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System;

public delegate void Action();

public class IngameHUD : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject winPanel;

    [SerializeField] private GameObject losePanel;

    [SerializeField] private UISettings uiSettingsData;

    [SerializeField] private Transform actionPanel;

    [SerializeField] private Transform statPanel;

    [SerializeField] private GameObject actionPrefab;

    [SerializeField] private GameObject statPrefab;

    [SerializeField] private Image previewImage;

    private Sprite defaultPreview;

    void Start()
    {
        defaultPreview = previewImage.sprite;
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			pauseMenu.SetActive(true);
		}
	}

    public void Clear()
    {
        foreach (Transform child in actionPanel)
            Destroy(child.gameObject);

        foreach (Transform child in statPanel)
            Destroy(child.gameObject);

        previewImage.sprite = defaultPreview;
    }
    public void Refresh( UnitData data, List<Action> actions )
    {
        // Destroy actual widgets
        foreach (Transform child in actionPanel)
            Destroy(child.gameObject);

        foreach (Transform child in statPanel)
            Destroy(child.gameObject);

        // Set preview image
        previewImage.sprite = data.preview;

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
        foreach( KeyValuePair<StatType, float> entry in data.stats )
        {
            GameObject statGO = Instantiate(statPrefab) as GameObject;
            statGO.transform.SetParent(statPanel);

            statGO.GetComponentInChildren<Text>().text = entry.Value.ToString();
            
            statGO.GetComponentInChildren<Image>().sprite = uiSettingsData.statSprites[entry.Key];
        }
    }

    public void ShowWinMessage()
    {
        winPanel.SetActive(true);
    }

    internal void ShowLoseMessage()
    {
        losePanel.SetActive(true);
    }
}
