using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Compass : MonoBehaviour {

	public float offset = 10f;
	private RectTransform parentRectTr;

	// Use this for initialization
	void Start () {
		parentRectTr = transform.parent.gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		PlaceAtCorner(); // specially for editor, if screenRatio changes

		// Rotate according to camera
		if (Input.GetKey(KeyCode.Q))
			gameObject.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 100);
		else if (Input.GetKey(KeyCode.E))
			gameObject.transform.RotateAround(transform.position, transform.up, -Time.deltaTime * 100);
	}

	private void PlaceAtCorner() {
		transform.localPosition = new Vector3(
			-parentRectTr.rect.size.x / 2f + transform.localScale.x / 2f + offset,
			parentRectTr.rect.size.y / 2f - transform.localScale.z / 2f - offset,
			0f
		);
	}
}
