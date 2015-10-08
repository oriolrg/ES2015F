using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResizeControllerStats : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float width = GetComponent<RectTransform> ().rect.width;
        float height = GetComponent<RectTransform>().rect.height;

        GetComponent<GridLayoutGroup> ().cellSize = new Vector2(0.2f * width,0.2f * width/3.0f);
		
		GetComponent<GridLayoutGroup> ().spacing = new Vector2(0.05f * width,0.05f * width);
		
		GetComponent<GridLayoutGroup> ().padding = new RectOffset ((int)(0.1f * height), (int)(0.1f * height), (int)(0.1f * height), (int)(0.1f * height));
	
	}
}
