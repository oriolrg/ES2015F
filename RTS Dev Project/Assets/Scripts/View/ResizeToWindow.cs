using UnityEngine;
using UnityEngine.UI;

public class ResizeToWindow : MonoBehaviour
{
    public float height = 0;
    public float heightPercent = 0.9f;
    public float spacing = 0.05f;
    public float padding = 0.05f;

    private int originalWidth;
    private int originalHeight;

    void Start()
    {
        originalWidth = Screen.width;
        originalHeight = Screen.height;

        height = GetComponent<RectTransform>().rect.height;

        GetComponent<GridLayoutGroup>().cellSize = heightPercent * height * Vector2.one;

        GetComponent<GridLayoutGroup>().spacing = spacing * height * Vector2.one;

        GetComponent<GridLayoutGroup>().padding = new RectOffset((int)(padding * height), (int)(padding * height), (int)(padding * height), (int)(padding * height));
    }

	void Update ()
    {
        // resize UI elements if the Screen has changed its size.
        //if (Screen.width != originalWidth || Screen.height != originalHeight)
        //{
            float height = GetComponent<RectTransform>().rect.height;

            GetComponent<GridLayoutGroup>().cellSize = heightPercent * height * Vector2.one;

            GetComponent<GridLayoutGroup>().spacing = spacing * height * Vector2.one;

            GetComponent<GridLayoutGroup>().padding = new RectOffset((int)(padding * height), (int)(padding * height), (int)(padding * height), (int)(padding * height));
        //}
	}
}
