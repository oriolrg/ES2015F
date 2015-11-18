using UnityEngine;
using System.Collections;

public class DestroyOnExpend : MonoBehaviour {

	public int amount;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		if (amount <= 0) {
            AI.Instance.deleteResource(this.gameObject);
			Destroy (this.gameObject);
		}
	}
}
