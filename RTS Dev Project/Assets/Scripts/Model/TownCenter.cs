using UnityEngine;
using System.Collections.Generic;

public class TownCenter : Unit 
{
    [SerializeField] private GameObject villagerPrefab;

    protected override List<Command> defineCommands()
    {
        return new List<Command>() { CreateUnit, DestroyBuilding };
    }



    public void CreateUnit()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            Ray ray = new Ray(transform.position - 3 * transform.up + 10*transform.forward, -Vector3.up);

            bool freeSpaceFound = false;

            RaycastHit hitInfo = new RaycastHit();

            while (!freeSpaceFound)
            {
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.tag != "Ground")
                    {
                        ray.origin = ray.origin + Vector3.right * 2;
                    }
                    else
                        freeSpaceFound = true;
                    
                }
            }
            Instantiate(villagerPrefab, hitInfo.point, Quaternion.identity);
        }
    }

    void DestroyBuilding()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GameController.Instance.removeUnit(gameObject);
            GetComponent<Animator>().SetBool("dead", true);
            Destroy(gameObject, 3);
        }
    }

    void Upgrade()
    {
        if (!inConstruction) //Disable the action if the villager is constructing a buliding.
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    
}
