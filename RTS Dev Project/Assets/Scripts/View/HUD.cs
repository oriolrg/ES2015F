using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField] private HUDData data;
    [SerializeField] private List<Image> panels;
    [SerializeField] private List<Text> texts;
    [SerializeField] private ResourceTextDictionary resourceTexts;
    [SerializeField] private ResourceTextDictionary resourceCosts;
    [SerializeField] private ActionGroupPanelDictionary actionGroupPanels;
    [SerializeField] private Image flagImage;
    [SerializeField] RectTransform creationPanel;
    [SerializeField] RectTransform troopPanel;
    [SerializeField] RectTransform rightPanel;
    [SerializeField] RectTransform resourceCostPanel;
    [SerializeField] Text descriptionText;
    [SerializeField] Image previewImage;
    [SerializeField] private RectTransform healthImage;
    [SerializeField] private Text nameText;

    
    public Sprite panelSprite;

    //Test
    void Start()
    {
        setCivilization(Civilization.Greeks);

    }

    // Changes the UI depending on the chosen civilization
    public void setCivilization( Civilization civilization )
    {
        // Load the civilization data from UISettings
        CivilizationData civilizationData = data.civilizationDatas[civilization];

        Sprite flag = civilizationData.FlagSprite;
        Sprite panel = civilizationData.PanelSprite;
        Font font = civilizationData.font;

        // Store the civilization panel sprite for creating future dynamic panels
        this.panelSprite = panel;

        // Change the sprite of each panel
        foreach ( Image image in panels )
        {
            image.sprite = panel;
        }

        //Change the font of each text
        foreach( Text text in texts )
        {
            text.font = font;
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

            // Update preview image and name
            previewImage.sprite = focusedUnit.Preview;
            nameText.text = focusedUnit.name;

            

            // Update Action buttons
            List<ActionData> actionDatas = focusedUnit.getActionDatas();
            foreach (ActionData actionData in actionDatas)
            {
                GameObject block = Instantiate(data.blockPrefab) as GameObject;
                
                Image background = block.GetComponent<Image>();
                background.sprite = panelSprite;

                Image foreground = block.transform.GetChild(0).GetComponent<Image>();
                foreground.sprite = actionData.sprite;

                // Get the parent panel for this panel depending on its group
                ActionGroup group = actionData.actionGroup;
                GameObject parent = actionGroupPanels[group];

                block.transform.SetParent(parent.transform);

                Button button = block.GetComponent<Button>();
                ActionData ad = actionData;

                button.interactable = !focusedUnit.getInConstruction(); //Disable the button if the unit is constructing a buliding. Doesnt work!!!

                button.onClick.AddListener(() =>
                {
                    focusedUnit.ActionClicked(ad);
                    updateDelayedActions(focusedUnit);
                });

                // Show resource costs when enter, hide when exit
                ShowResourceCostWhenEnter script = block.AddComponent<ShowResourceCostWhenEnter>();
                script.data = actionData;
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
        foreach (Transform child in creationPanel) Destroy(child.gameObject);
        foreach( KeyValuePair<ActionGroup,GameObject> kv in actionGroupPanels )
        {
            foreach( Transform child in kv.Value.transform )
            {
                Destroy(child.gameObject);
            }
        }
        nameText.text = "";
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
                    image.color = new Color(.6f, .8f, 1f, .5f);
                    focusFrame.transform.SetParent(block.transform.GetChild(0));
                }
            }           
        }
    }

    public void enterActionButton( ActionData data )
    {
        rightPanel.gameObject.SetActive(true);
        foreach(KeyValuePair<Resource,Text> kv in resourceCosts)
        {
            Resource resource = kv.Key;
            kv.Value.text = data.resourceCost[resource].ToString();
            Text text = resourceTexts[resource];
            int newvalue = (Int32.Parse(text.text) - data.resourceCost[resource]);
            text.text =  newvalue.ToString();
            if(data.resourceCost[resource] != 0)
                text.color = newvalue > 0 ? new Color(.3f, .3f, .3f, 1f) : new Color(.5f, 0f, 0f, 1f);
        }
        descriptionText.text = data.description;
    }

    public void exitActionButton(ActionData data)
    {
        foreach (KeyValuePair<Resource, Text> kv in resourceCosts)
        {
            Resource resource = kv.Key;
            kv.Value.text = data.resourceCost[resource].ToString();
            Text text = resourceTexts[resource];
            text.text = (Int32.Parse(text.text) + data.resourceCost[resource]).ToString();
            text.color = new Color(0f, 0f, 0f, 1f);
        }
    }

    public void hideRightPanel()
    {
        rightPanel.gameObject.SetActive(false);
    }
}