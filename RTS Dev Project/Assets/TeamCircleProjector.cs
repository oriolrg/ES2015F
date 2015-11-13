using UnityEngine;
using System.Collections;

public class TeamCircleProjector : MonoBehaviour {

    private GameObject projector;

    // Use this for initialization
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init()
    {
        CharacterController character = GetComponentInParent<CharacterController>();
        if (character != null)
        {
            parentBounds = character.bounds;
        }
        else
        {
            BoxCollider box = GetComponentInParent<BoxCollider>();
            if (box != null)
            {
                parentBounds = box.bounds;
            }
        }

        projector = GetComponent<Projector>();

        if (parentBounds != null && projector != null)
        {
            projector.orthographicSize = Mathf.Max(parentBounds.extents.x, parentBounds.extents.z);
        }
    }
}
