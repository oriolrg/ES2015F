﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [SerializeField] private UISettings data;
    [SerializeField] private List<Image> panels;
    [SerializeField] private Image flagImage;
    [SerializeField] private ResourceTextDictionary resourceTexts;
    [SerializeField] RectTransform troopPanel;
    [SerializeField] RectTransform actionPanel;
    [SerializeField] Image previewImage;
    [SerializeField] private RectTransform healthImage;

    // Test
    public Sprite panelSprite;
    public Unit testUnit;
    public Troop testTroop;
    
    void Start()
    {
        setCivilization(Civilization.Greeks);
        updateResource(Resource.Food, 100);
        updateResource(Resource.Wood, 100);
        updateResource(Resource.Metal, 100);
        updateResource(Resource.Population, 2);
        updateHealth(testUnit);
        updateSelection(testTroop);
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
        foreach( Unit unit in troop.units )
        {
            GameObject block = Instantiate(data.blockPrefab) as GameObject;
            block.transform.SetParent(troopPanel);

            Image background = block.GetComponent<Image>();
            background.sprite = panelSprite;

            Image foreground = block.transform.GetChild(0).GetComponent<Image>();
            foreground.sprite = unit.Preview;
        }
        Unit focusedUnit = troop.FocusedUnit;
        previewImage.sprite = focusedUnit.Preview;

        for (int i = 0; i < focusedUnit.getData().actions.Count; i++ )
        {
            ActionData actionData = focusedUnit.getData().actions[i];
            Command command = focusedUnit.getCommands()[i];
            GameObject block = Instantiate(data.blockPrefab) as GameObject;
            block.transform.SetParent(actionPanel);

            Image background = block.GetComponent<Image>();
            background.sprite = panelSprite;

            Image foreground = block.transform.GetChild(0).GetComponent<Image>();
            foreground.sprite = actionData.sprite;

            Button button = block.GetComponent<Button>();
            button.onClick.AddListener(() => { command(); });
        }
    }

    // Set the health ratio : current / total as length of the health image
    public void updateHealth( Unit unit )
    {
        print(unit.HealthRatio);
        healthImage.localScale = new Vector3(unit.HealthRatio, 1, 1 );
    }

    // Repaint delayed actions when a new one is created. Actions disappear automatically
    public void updateDelayedActions( Unit unit )
    {

    }
}