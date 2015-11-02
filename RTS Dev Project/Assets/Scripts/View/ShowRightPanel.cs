using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowResourceCostWhenEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public UnitData data;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.Instance.OnActionButtonEnter(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.OnActionButtonExit(data);
    }
}
