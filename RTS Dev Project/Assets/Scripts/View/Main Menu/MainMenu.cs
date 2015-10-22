using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public void NewGame(string scene){
		Application.LoadLevel (scene);
	}

	public void Quit(){
		print ("Quit");
		Application.Quit ();
	}
}
