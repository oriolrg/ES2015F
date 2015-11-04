using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	[SerializeField] private string label = "Loading";
	[SerializeField] private float waitDot = 1f;
	private Text text;
	private int dots = 3;

	private float time;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		text.text = label + getDotsLabel();
		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeInHierarchy)
			return;

		time += Time.deltaTime;

		if (time > waitDot){
			time -= waitDot;

			dots++;
			if (dots > 3)
				dots = 1;

			text.text = label + getDotsLabel();
		}
	}

	private string getDotsLabel(){
		string res;
		if (dots == 1)
			res = ".  ";
		else if (dots == 2)
			res = ".. ";
		else
			res = "...";

		return res;
	}
}
