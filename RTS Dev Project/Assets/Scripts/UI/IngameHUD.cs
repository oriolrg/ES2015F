using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class IngameHUD : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject winPanel;

    [SerializeField] private GameObject losePanel;

    [SerializeField] private UISettings uiSettingsData;

    [SerializeField] private Transform actionPanel;
    [SerializeField]
    private Transform creationPanel;

    [SerializeField] private Text foodText;
    [SerializeField] private Text woodText;
    [SerializeField] private Text metalText;

    [SerializeField] private GameObject actionPrefab;

    [SerializeField] private GameObject creationPrefab;

    [SerializeField] private Image previewImage;

    [SerializeField] private Sprite defaultPreview;

    public void Clear()
    {
        foreach (Transform child in actionPanel)
            Destroy(child.gameObject);

        previewImage.sprite = defaultPreview;
    }
    public void Refresh( UnitData data, List<Command> actions )
    {
        // Destroy actual widgets
        foreach (Transform child in actionPanel)
            Destroy(child.gameObject);

        // Set preview image
        previewImage.sprite = data.preview;

        // Set action buttons
        for ( int i = 0; i < data.actionSprites.Count; i++ )
        {
            GameObject actionGO = Instantiate(actionPrefab) as GameObject;
            actionGO.transform.SetParent( actionPanel );

            Command act = actions[i];

            actionGO.GetComponent<Button>().onClick.AddListener(() => { act(); } );
            Transform insideImage = actionGO.transform.GetChild(0);
            insideImage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            insideImage.GetComponent<Image>().sprite = data.actionSprites[i];
        }

        
    }

    internal void addCreation(Creation creation)
    {
        GameObject creationGO = Instantiate(creationPrefab) as GameObject;
        creationGO.transform.SetParent(creationPanel);

        Image creationImage = creationGO.transform.GetChild(0).GetComponent<Image>();

        creationImage.sprite = creation.sprite;

        UnfillWithTime script = creationGO.GetComponentInChildren<UnfillWithTime>();
        script.time = creation.time;
        script.callback = creation.action;

        script.enabled = true;

    }

    public void ShowWinMessage()
    {
        winPanel.SetActive(true);
    }

    internal void ShowLoseMessage()
    {
        losePanel.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
}
