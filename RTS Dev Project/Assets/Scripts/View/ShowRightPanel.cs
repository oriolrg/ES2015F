using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowResourceCostWhenEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ActionData data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.Instance.enterActionButton(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.exitActionButton(data);
    }
}
