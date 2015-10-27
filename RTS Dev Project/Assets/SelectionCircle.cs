using UnityEngine;
using System.Collections;

public class SelectionCircle : MonoBehaviour {
    private Renderer parentRenderer;
    private Projector projector;

	// Use this for initialization
	void Start () {
        parentRenderer = GetComponentInParent<Renderer>();
        projector = GetComponent<Projector>();
        if (parentRenderer != null)
        {
            if (projector != null)
            {
                projector.orthographicSize = parentRenderer.bounds.size.x + 1;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
