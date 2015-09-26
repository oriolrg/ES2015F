using UnityEngine;
using System.Collections.Generic;



public class Unit : MonoBehaviour 
{
    [SerializeField] private IngameHUD hud;

    void Start()
    {
        hud.setActions( new List<Action>() { CreateUnit, DestroyBuilding, Upgrade });
        hud.refresh();
    }

    void CreateUnit()
    {
        print("create");
    }

    void DestroyBuilding()
    {
        print("destroy");
    }

    void Upgrade()
    {
        print("upgrade");
    }


}
