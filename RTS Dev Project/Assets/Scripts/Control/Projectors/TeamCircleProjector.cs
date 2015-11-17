using UnityEngine;
using System.Collections;
using System;

public class TeamCircleProjector : MonoBehaviour {
    private Bounds parentBounds;
    private Projector projector;
    private LOSEntity los;
    public LOSEntity.RevealStates revealState;

    // Use this for initialization
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (los == null)
        {
            los = GetComponentInParent<LOSEntity>();
            if (los != null)
            {
                revealState = los.RevealState;
                newState();
            }
        }
        else
        {
            if (revealState != los.RevealState)
            {
                revealState = los.RevealState;
                newState();
            }
        }
    }

    private void newState()
    {
        if (revealState == LOSEntity.RevealStates.Hidden)
            projector.enabled = false;
        if (revealState == LOSEntity.RevealStates.Fogged)
            projector.enabled = true;
        if (revealState == LOSEntity.RevealStates.Unfogged)
            projector.enabled = true;
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
            projector.orthographicSize = Mathf.Max(parentBounds.extents.x, parentBounds.extents.z)*2.5f;
        }
    }

    public void initWithTeamColor(Identity iden)
    {
        init();
        projector.material = DataManager.Instance.civilizationDatas[iden.civilization].material;
    }
}
