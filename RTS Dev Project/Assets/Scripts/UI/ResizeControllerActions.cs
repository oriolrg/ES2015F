using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResizeControllerActions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float height = GetComponent<RectTransform> ().rect.height;

		GetComponent<GridLayoutGroup> ().cellSize = new Vector2(0.4f * height,0.4f * height);

		GetComponent<GridLayoutGroup> ().spacing = new Vector2(0.1f * height,0.1f * height);

		GetComponent<GridLayoutGroup> ().padding = new RectOffset ((int)(0.1f * height), (int)(0.1f * height), (int)(0.1f * height), (int)(0.1f * height));

	}
}
