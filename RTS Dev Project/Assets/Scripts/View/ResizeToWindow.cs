using UnityEngine;
using UnityEngine.UI;

public class ResizeToWindow : MonoBehaviour
{
    public float height = 0;
    public float heightPercent = 0.8f;
    public float spacing = 0.1f;
    public float padding = 0.1f;

	void Update ()
    {
        // This can be CPU intensive, could check if window has changed size bt not for now.
        float height = GetComponent<RectTransform>().rect.height;

        GetComponent<GridLayoutGroup>().cellSize = heightPercent * height * Vector2.one;

        GetComponent<GridLayoutGroup>().spacing = spacing * height * Vector2.one;

        GetComponent<GridLayoutGroup>().padding = new RectOffset((int)(padding * height), (int)(padding * height), (int)(padding * height), (int)(padding * height));

	}
}
