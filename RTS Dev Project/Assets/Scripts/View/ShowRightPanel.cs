using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowRightPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ResourceValueDictionary zeroResources = new ResourceValueDictionary() { { Resource.Food, 0 }, { Resource.Wood, 0 }, { Resource.Population, 0 }, { Resource.Metal, 0 } };

    public String description;
    public ResourceValueDictionary resourceCost;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.Instance.OnActionButtonEnter(description, resourceCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.OnActionButtonExit(description, resourceCost);
    }
}
