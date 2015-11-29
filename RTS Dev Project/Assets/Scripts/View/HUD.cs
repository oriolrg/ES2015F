using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Linq;

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
    [SerializeField] RectTransform controlPanel;
    [SerializeField] RectTransform countdownPanel;
    [SerializeField] Text descriptionText;
    [SerializeField] Image previewImage;
    [SerializeField] private RectTransform healthImage;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private Countdown countdownText;
    [SerializeField] private Text victoryCondition;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] public GameObject gameMenu;

    private Sprite panelSprite;

    private Dictionary<Civilization, Color> civilizationColors;



    void Start()
    {
		civilizationColors = new Dictionary<Civilization,Color> ();

		foreach (KeyValuePair<Civilization, CivilizationData> kv in DataManager.Instance.civilizationDatas) {
            civilizationColors.Add(kv.Key, kv.Value.color);
		}
    }

    // Changes the UI depending on the chosen civilization
    public void setCivilization( Civilization civilization )
    {
        // Load the civilization data from UISettings
        CivilizationData civilizationData = DataManager.Instance.civilizationDatas[civilization];

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

        GameObject focusedUnit = troop.FocusedUnit;

        if (focusedUnit != null)
        {
            // Get focused unit of the troop
            Identity identity = troop.FocusedUnit.GetComponent<Identity>();

            if (identity == null) return;

            UnitData unitData = DataManager.Instance.unitDatas[identity.unitType];
            // Update preview image and name
            previewImage.sprite = unitData.preview;
            nameText.text = identity.name;



            // Update Action buttons
            UnitType unitType = identity.unitType;

            if (actionsData == null) print("bad");

            List<UnitType> creations = actionsData.creationPermissions[unitType];
            for (int i = 0; i < creations.Count; i++)
            {
                UnitType type = creations[i];
                // get the unit data of the unit that can be created
                UnitData creationData = DataManager.Instance.unitDatas[type];

                // Create a block prefab with the image of the action
                addBlock(createPanel, creationData.preview, creationData.description, creationData.resourceCost, () => { GameController.Instance.OnCreate(identity, type); });


            }

            // if the unittype is mobile add move actons
            if (!unitType.isBuilding())
            {
                List<MoveData> movements = actionsData.movements;
                for (int i = 0; i < movements.Count; i++)
                {
                    MoveData movement = movements[i];
                    addBlock(movePanel, movement.preview, movement.description, null, () => { movement.codeToExecute.Invoke(); });

                }

            }

            // Add sacrifice action if it is not an objective
            Objective objective = focusedUnit.GetComponent<Objective>();
            if (objective == null)
            {
                List<SpecialData> specials = actionsData.specials;
                for (int i = 0; i < specials.Count; i++)
                {
                    SpecialData special = specials[i];
                    addBlock(specialPanel, special.preview, special.description, null, () => { special.codeToExecute.Invoke(); });
                }

                updateInteractable(focusedUnit);

                updateControl(focusedUnit);
                updateHealth(focusedUnit);
                updateDelayedActions(focusedUnit);

                previewImage.color = Color.white;

            }
            else
            {
                // Add controller color to preview if its an objective
                Civilization civilization = objective.Controller;

                previewImage.color = DataManager.Instance.civilizationDatas[civilization].color;
            }
        }
        
    }

    public void updateInteractable(GameObject focusedUnit)
    {
        foreach(Transform child in createPanel)
        {
            
            BuildingConstruction bc = focusedUnit.GetComponent<BuildingConstruction>();
            if (bc != null)
            {
                child.GetComponent<Button>().interactable = !bc.getConstructionOnGoing();
            }
            
        }

        foreach (Transform child in movePanel)
        {

            BuildingConstruction bc = focusedUnit.GetComponent<BuildingConstruction>();
            if (bc != null)
            {
                child.GetComponent<Button>().interactable = !bc.getConstructionOnGoing();
            }

        }

        foreach (Transform child in specialPanel)
        {

            BuildingConstruction bc = focusedUnit.GetComponent<BuildingConstruction>();
            if (bc != null)
            {
                child.GetComponent<Button>().interactable = !bc.getConstructionOnGoing();
            }

        }

    }

    // Set the health ratio : current / total as length of the health image
    public void updateHealth( GameObject unit )
    {
        Health script = unit.GetComponent<Health>();
        if (script != null)
            healthImage.localScale = new Vector3(script.HealthRatio, 1, 1);
        else
            healthImage.localScale = Vector3.zero;
    }

    // Repaint delayed actions when a new one is created. Actions disappear automatically
    public void updateDelayedActions( GameObject unit )
    {
        // Destroy all current actions
        foreach (Transform child in creationPanel)
            Destroy(child.gameObject);

        // Loop the queued actions
        DelayedActionQueue script = unit.GetComponent<DelayedActionQueue>();
        if (script == null) return;

        Queue<Action> queuedActions = script.Queue;

        foreach(Action action in queuedActions)
        {
            GameObject block = addBlock(creationPanel, action.Preview, () => { print("cancel action"); });
            
            // Add time frame to each block
            GameObject timeFrame = Instantiate(data.overlappedTimeFrame) as GameObject;
            timeFrame.transform.SetParent(block.transform);

            UnfillWithTime filling = timeFrame.GetComponent<UnfillWithTime>();
            filling.action = action;
        }
    }

	public void updateControl(GameObject unit)
	{
        Objective objective = unit.GetComponent<Objective>();

        if (objective != null)
        {
            foreach (Transform child in controlPanel) Destroy(child.gameObject);

            CivilizationValueDictionary control = objective.control;

            if ( control.Count > 0 && control.Max(x => x.Value) > .99f)
                Invoke("DisappearControl", 3);
            else
            {
                CancelInvoke("DisappearControl");
                controlPanel.gameObject.SetActive(true);
            }
            float accumulatedX = 0;
            foreach (KeyValuePair<Civilization, float> kv in control)
            {
                Civilization c = kv.Key;
                float value = kv.Value;

                GameObject controlGO = Instantiate(data.controlPrefab) as GameObject;

                controlGO.transform.SetParent(controlPanel);

                controlGO.GetComponent<Image>().color = civilizationColors[c];

                RectTransform rect = controlGO.GetComponent<RectTransform>();

                rect.anchorMin = new Vector2(accumulatedX, 0);
                accumulatedX += value;
                if (accumulatedX > .99f)
                    accumulatedX = 1;
                rect.anchorMax = new Vector2(accumulatedX, 1);

                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

            }
        }
        else
        {
            DisappearControl();
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

            UnitData unitData = DataManager.Instance.unitDatas[identity.unitType];


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
    
    public void OnActionButtonEnter( String description, ResourceValueDictionary resourceCost )
    {
        rightPanel.gameObject.SetActive(true);
        foreach(KeyValuePair<Resource,Text> kv in resourceCosts)
        {
            Resource resource = kv.Key;
            kv.Value.text = resourceCost[resource].ToString();
            Text text = resourceTexts[resource];
            int newvalue = (Int32.Parse(text.text) - resourceCost[resource]);
            text.text =  newvalue.ToString();
            if(resourceCost[resource] != 0)
                text.color = newvalue > 0 ? new Color(.3f, .3f, .3f, 1f) : new Color(.5f, 0f, 0f, 1f);
        }
        descriptionText.text = description;
    }

    public void OnActionButtonExit(String description, ResourceValueDictionary resourceCost)
    {
        foreach (KeyValuePair<Resource, Text> kv in resourceCosts)
        {
            Resource resource = kv.Key;
            kv.Value.text = resourceCost[resource].ToString();
            Text text = resourceTexts[resource];
            text.text = (Int32.Parse(text.text) + resourceCost[resource]).ToString();
            text.color = new Color(0f, 0f, 0f, 1f);
        }
    }
    
    public void hideRightPanel()
    {
        rightPanel.gameObject.SetActive(false);
    }

    private void DisappearControl()
	{
		controlPanel.gameObject.SetActive (false);
	}

    private GameObject addBlock( Transform parent, Sprite image, String description, ResourceValueDictionary resourceCost, UnityAction callback)
    {
        GameObject block = Instantiate(data.blockPrefab) as GameObject;

        Image background = block.GetComponent<Image>();
        background.sprite = panelSprite;
        Image foreground = block.transform.GetChild(0).GetComponent<Image>();
        foreground.sprite = image;
        
        block.AddComponent<ShowRightPanel>();
        block.GetComponent<ShowRightPanel>().description = description;
        block.GetComponent<ShowRightPanel>().resourceCost = resourceCost == null ? ShowRightPanel.zeroResources : resourceCost;

        block.transform.SetParent(parent);

        // Add listener when clicked
        Button button = block.GetComponent<Button>();

        button.onClick.AddListener(callback);



        return block;
    }

    private GameObject addBlock(Transform parent, Sprite image, UnityAction callback)
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

    public void showMessageBox( string text)
    {
        messageBox.SetActive(true);
        messageBox.transform.GetChild(0).GetComponent<Text>().text = text;
        CancelInvoke("hideMessageBox");
        Invoke("hideMessageBox", 3);
    }

    public void hideMessageBox()
    {
        messageBox.SetActive(false);
    }

    public void startCountdown(Victory victory, Civilization winner )
    {
        countdownPanel.gameObject.SetActive(true);
        victoryCondition.text = string.Format("{0}: {1}", victory.ToString(), DataManager.Instance.civilizationDatas[winner].name);
        countdownText.setTimer( victory.countdownTime(), () => { winPanel.SetActive(true); });
    }

    public void stopCountdown(Victory victory)
    {
        countdownPanel.gameObject.SetActive(false);
        countdownText.stop();
    }

	public void updateRightPanel(GameObject go)
	{
		if(go.tag != "Enemy"){
			rightPanel.gameObject.SetActive(true);
			CollectResources collectResources = go.GetComponent<CollectResources>();
			if(collectResources!=null){
				foreach( Resource resource in Enum.GetValues(typeof(Resource)))
				{
					resourceCosts[resource].text = collectResources.resourceBank[resource].ToString();
				}
			}
		}
	}
    /*
	public void ShowWinMessage(){
		winPanel.SetActive(true);
	}
	public void ShowLoseMessage(){
		losePanel.SetActive(true);
	}*/
}
