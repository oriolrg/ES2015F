using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			gameObject.SetActive(false);
	}

	void OnEnable() { Time.timeScale = 0; }
	void OnDisable() { Time.timeScale = 1; }
}
