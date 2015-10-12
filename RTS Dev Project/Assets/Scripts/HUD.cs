using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [SerializeField] private UISettings data;
    [SerializeField] private List<Image> panels;
    [SerializeField] private Image flagImage;

    private Sprite panel;

    void Start()
    {
        setCivilization(Civilization.Egyptians);
    }

    // Changes the UI depending on the chosen civilization
    public void setCivilization( Civilization civilization )
    {
        // Load the civilization data from UISettings
        CivilizationData civilizationData = data.civilizationDatas[civilization];

        Sprite flag = civilizationData.FlagSprite;
        Sprite panel = civilizationData.PanelSprite;

        // Store the civilization panel sprite for creating future dynamic panels
        this.panel = panel;

        // Change the sprite of each panel
        foreach ( Image image in panels )
        {
            image.sprite = panel;
        }
        
        // Change the flag sprite
        flagImage.sprite = flag;
    }

    public void updateResource( Resource resource, int value )
    {

    }

    public void updateSelection( Troop troop )
    {

    }

    public void updateLife( int value )
    {

    }
}