using UnityEngine;
using System.Collections;

public class updateGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("patata");
		AstarPath.active.Scan();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
