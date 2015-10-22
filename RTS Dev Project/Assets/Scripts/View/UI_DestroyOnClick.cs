using UnityEngine;
using UnityEngine.UI;

public class UI_DestroyOnClick : MonoBehaviour
{
    public Button button;

	void Start ()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { Destroy(gameObject); });
	}
}
