using UnityEngine;
using UnityEngine.EventSystems;

public class HideRightPanel : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.hideRightPanel();
    }
}
