using UnityEngine;
using System.Collections;

abstract public class ChoicePickerChangeStateListener : MonoBehaviour {

	abstract public void OnChangeState (string state);
	abstract public void OnChangeActive (bool active);
}
