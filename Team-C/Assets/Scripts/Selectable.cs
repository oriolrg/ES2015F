using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Selectable : MonoBehaviour
{
    protected List<Action> actions;
    [SerializeField] protected Dictionary<string, float> stats;

    [SerializeField] protected IngameHUD hud;

    [SerializeField] protected Sprite previewImage;

    void Start()
    {
        if ( hud == null )
            hud = Camera.main.GetComponent<IngameHUD>();
    }

    void OnMouseDown()
    {
        hud.refresh(actions, stats, previewImage);
    }
}
