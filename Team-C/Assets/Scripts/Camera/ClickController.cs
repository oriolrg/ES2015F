using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {

    public Transform prefab;


    void Start()
    {

    }


    void Update()
    {
        //When the left button of the mouse is clicked, get the position and create a prefab there.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(prefab, new Vector3(hit.point.x, prefab.position.y, hit.point.z), prefab.rotation);
            }

        }

    }
}
