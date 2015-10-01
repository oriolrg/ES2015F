using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {

    public Transform prefab;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
