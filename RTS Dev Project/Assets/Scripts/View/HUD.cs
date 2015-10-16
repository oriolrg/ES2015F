using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField] private HUDData data;
    [SerializeField] private List<Image> panels;
    [SerializeField] private Image flagImage;
    [SerializeField] private ResourceTextDictionary resourceTexts;
    [SerializeField] RectTransform creationPanel;
    [SerializeField] RectTransform troopPanel;
    [SerializeField] RectTransform actionPanel;
    [SerializeField] Image previewImage;
    [SerializeField] private RectTransform healthImage;

    // Test
    public Sprite panelSprite;
 
    
    void Start()
    {
        setCivilization(Civilization.Egyptians);
        updateResource(Resource.Food, 100);
        updateResource(Resource.Wood, 100);
        updateResource(Resource.Metal, 100);
        updateResource(Resource.Population, 2);

    }



    // End test

    // Changes the UI depending on the chosen civilization
    public void setCivilization( Civilization civilization )
    {
        // Load the civilization data from UISettings
        CivilizationData civilizationData = data.civilizationDatas[civilization];

        Sprite flag = civilizationData.FlagSprite;
        Sprite panel = civilizationData.PanelSprite;

        // Store the civilization panel sprite for creating future dynamic panels
        this.panelSprite = panel;

        // Change the sprite of each panel
        foreach ( Image image in panels )
        {
            image.sprite = panel;
        }
        
        // Change the flag sprite
        flagImage.sprite = flag;
    }

    // Updates the text of a resource. resourceTexts should be filled within inspector
    public void updateResource( Resource resource, int value )
    {
        // Get the Text component that corresponds to this resource
        Text text = resourceTexts[resource];

        // Show the value
        text.text = value.ToString();
    }

    public void updateSelection( Troop troop )
    {
        ClearSelection();
        // Update troop panel (private)
        setTroopPreview( troop );

        if (troop.FocusedUnit != null)
        {
            // Get focused unit of the troop
            Unit focusedUnit = troop.FocusedUnit.GetComponent<Unit>();

            // Update preview image and focus
            previewImage.sprite = focusedUnit.Preview;

            

            // Update Action buttons
            List<ActionData> actionDatas = focusedUnit.getActionDatas();
            foreach (ActionData actionData in actionDatas)
            {
                GameObject block = Instantiate(data.blockPrefab) as GameObject;
                block.transform.SetParent(actionPanel);

                Image background = block.GetComponent<Image>();
                background.sprite = panelSprite;

                Image foreground = block.transform.GetChild(0).GetComponent<Image>();
                foreground.sprite = actionData.sprite;



                Button button = block.GetComponent<Button>();
                ActionData ad = actionData;

                button.interactable = !focusedUnit.getInConstruction(); //Disable the button if the unit is constructing a buliding.

                button.onClick.AddListener(() =>
                {
                    focusedUnit.ActionClicked(ad);
                    updateDelayedActions(focusedUnit);
                });
            }

            updateHealth(focusedUnit);
            updateDelayedActions(focusedUnit);
        }
    }

    // Set the health ratio : current / total as length of the health image
    public void updateHealth( Unit unit )
    {
        healthImage.localScale = new Vector3(unit.HealthRatio, 1, 1 );
    }

    // Repaint delayed actions when a new one is created. Actions disappear automatically
    public void updateDelayedActions( Unit unit )
    {

        foreach (Transform child in creationPanel)
            Destroy(child.gameObject);

        Queue<Action> queuedActions = unit.Queue;

        foreach(Action action in queuedActions)
        {
            GameObject block = Instantiate(data.blockPrefab) as GameObject;
            block.transform.SetParent(creationPanel);

            Image background = block.GetComponent<Image>();
            background.sprite = panelSprite;

            Image foreground = block.transform.GetChild(0).GetComponent<Image>();
            ActionData a = action.Data;
            foreground.sprite = a.sprite;

            Button button = block.GetComponent<Button>();
            button.onClick.AddListener(() => { print("Cancel action"); });

            GameObject timeFrame = Instantiate(data.overlappedTimeFrame) as GameObject;
            timeFrame.transform.SetParent(block.transform.GetChild(0));

            UnfillWithTime script = timeFrame.GetComponent<UnfillWithTime>();
            script.action = action;
        }
    }

    public void ClearSelection()
    {
        foreach (Transform child in troopPanel) Destroy(child.gameObject);
        previewImage.sprite = panelSprite;
        foreach (Transform child in actionPanel) Destroy(child.gameObject);
        foreach (Transform child in creationPanel) Destroy(child.gameObject);
    }

    public void setTroopPreview( Troop troop )
    {
        for( int i = 0; i < troop.units.Count; i++ )
        {
            GameObject unit = troop.units[i];

            // if actually a unit ( will be fixed )
            if (unit.GetComponent<Unit>() != null)
            {
                GameObject block = Instantiate(data.blockPrefab) as GameObject;
                block.transform.SetParent(troopPanel);

                Image background = block.GetComponent<Image>();
                background.sprite = panelSprite;

                Image foreground = block.transform.GetChild(0).GetComponent<Image>();
                foreground.sprite = unit.GetComponent<Unit>().Preview;

                //Paint focus fram
                if (unit == troop.FocusedUnit)
                {
                    GameObject focusFrame = new GameObject("focus frame");
                    Image image = focusFrame.AddComponent<Image>();
                    image.sprite = data.focusSprite;
                    image.color = new Color(.1f, 1, .1f, .5f);
                    focusFrame.transform.SetParent(block.transform.GetChild(0));
                }
            }           
        }
    }
}