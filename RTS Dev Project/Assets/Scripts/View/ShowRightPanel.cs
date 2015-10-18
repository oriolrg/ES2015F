using UnityEngine;
using UnityEngine.EventSystems;

public class ShowResourceCostWhenEnter : MonoBehaviour, IPointerEnterHandler
{
    public ActionData data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.Instance.showRightPanel(data);
    }
    
}
