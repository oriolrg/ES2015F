using UnityEngine;
using System.Collections;

public class SelectionCircle : MonoBehaviour {
    private Renderer parentRenderer;
    private Projector projector;

	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void init()
    {
        parentRenderer = GetComponentInParent<Renderer>();
        projector = GetComponent<Projector>();
        if (parentRenderer != null && projector != null)
        {
                projector.orthographicSize = parentRenderer.bounds.size.x + 1;
        }
    }
}
