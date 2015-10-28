using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class HUD : MonoBehaviour
{
    [SerializeField] private ActionsData actionsData;
    [SerializeField] private HUDData data;
    [SerializeField] private List<Image> panels;
    [SerializeField] private List<Text> texts;
    [SerializeField] private ResourceTextDictionary resourceTexts;
    [SerializeField] private ResourceTextDictionary resourceCosts;
    [SerializeField] private Image flagImage;
    [SerializeField] RectTransform creationPanel;
    [SerializeField] RectTransform troopPanel;
    [SerializeField] RectTransform rightPanel;
    [SerializeField] RectTransform resourceCostPanel;
    [SerializeField] RectTransform createPanel;
    [SerializeField] RectTransform movePanel;
    [SerializeField] RectTransform specialPanel;
    [SerializeField] Text descriptionText;
    [SerializeField] Image previewImage;
    [SerializeField] private RectTransform healthImage;
    [SerializeField] private Text nameText;

    
    private Sprite panelSprite;

    void Start()
    {
        setCivilization(Civilization.Greeks);
    }

    // Changes the UI depending on the chosen civilization
    public void setCivilization( Civilization civilization )
    {
        // Load the civilization data from UISettings
        CivilizationData civilizationData = DataManager.Instance.civilizations[civilization];

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
            Identity focusedUnit = troop.FocusedUnit.GetComponent<Identity>();
            
            if (focusedUnit == null) return;

            UnitData unitData = focusedUnit.data;
            // Update preview image and name
            previewImage.sprite = unitData.preview;
            nameText.text = focusedUnit.name;



            // Update Action buttons
            UnitType unitType = focusedUnit.unit;

            List<UnitType> creations = actionsData.creationPermissions[unitType];
            for( int i = 0; i < creations.Count; i++ )
            {
                UnitType type = creations[i];
                // get the unit data of the unit that can be created
                UnitData toCreate = DataManager.Instance.civilizations[focusedUnit.civilization].units[type];

                // Create a block prefab with the image of the action
                GameObject block = addBlock(createPanel, toCreate.preview, () => { GameController.Instance.OnCreate(toCreate.prefab); });

                
            }

            // if the unittype is mobile add move actons
            if( !unitType.isBuilding() )
            {
                List<MoveData> movements = actionsData.movements;
                for( int i = 0; i < movements.Count; i++ )
                {
                    MoveData movement = movements[i];
                    addBlock(movePanel, movement.preview, () => { movement.codeToExecute.Invoke(); });
                }
            }

            // Add sacrifice action
            List<SpecialData> specials = actionsData.specials;
            for (int i = 0; i < specials.Count; i++)
            {
                SpecialData special = specials[i];
                addBlock(specialPanel, special.preview, () => { special.codeToExecute.Invoke(); });
            }

            //updateHealth(focusedUnit);
            //updateDelayedActions(focusedUnit);
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
            //ActionData a = action.Data;
            //foreground.sprite = a.sprite;

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
        foreach (Transform child in createPanel) Destroy(child.gameObject);
        foreach (Transform child in movePanel) Destroy(child.gameObject);
        foreach (Transform child in specialPanel) Destroy(child.gameObject);
        nameText.text = "";
    }

    public void setTroopPreview( Troop troop )
    {
        for( int i = 0; i < troop.units.Count; i++ )
        {
            // Get the Unit Data of the current unit
            GameObject unit = troop.units[i];

            Identity identity = unit.GetComponent<Identity>();

            if (identity == null) continue;

            UnitData unitData = identity.data;


            // Instantiate a block with the correct image
            GameObject block = Instantiate(data.blockPrefab) as GameObject;
            block.transform.SetParent(troopPanel);

            Image background = block.GetComponent<Image>();
            background.sprite = panelSprite;

            Image foreground = block.transform.GetChild(0).GetComponent<Image>();
            foreground.sprite = unitData.preview;

            // Additionally paint focus frame if its the focused unit
            if (unit == troop.FocusedUnit)
            {
                GameObject focusFrame = Instantiate(data.focusFrame) as GameObject;
                focusFrame.transform.SetParent(block.transform.GetChild(0));
            }
        }
        
    }
    
    public void OnActionButtonEnter( UnitData data )
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

    public void OnActionButtonExit(UnitData data)
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

    private GameObject addBlock( Transform parent, Sprite image, UnityAction callback)
    {
        GameObject block = Instantiate(data.blockPrefab) as GameObject;
        Image background = block.GetComponent<Image>();
        background.sprite = panelSprite;
        Image foreground = block.transform.GetChild(0).GetComponent<Image>();
        foreground.sprite = image;
        block.transform.SetParent(parent);

        // Add listener when clicked
        Button button = block.GetComponent<Button>();

        button.onClick.AddListener(callback);



        return block;
    }
}