using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{

    public float maxHealth;
    public float curHealth;

    public float minXValue;
    public float maxXValue;

    private Image visualHealth;

    public GameObject prefab;

    private GameObject g;
    private GameObject g1;

	private LOSEntity unitLOSEntity;

    // Use this for initialization
    void Start()
    {
        maxHealth = GetComponent<Health>().getMaxHealth();
        curHealth = GetComponent<Health>().getHealth();

        if (!GetComponent<Identity>().unitType.isBuilding())
        {
            g = Instantiate(prefab);
            g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);
            g1 = g.GetComponentInChildren<auxHealth>().gameObject;

            visualHealth = g.GetComponentInChildren<auxHealth>().i;
        }

		unitLOSEntity = gameObject.GetComponent<LOSEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (g == null)
        {
            if (GetComponent<Identity>().unitType.isBuilding())
            {
                //if (GetComponent<BuildingConstruction>().getPhase() == 2)
                if (!GetComponent<BuildingConstruction>().getConstructionOnGoing())
                {
                    g = Instantiate(prefab);
                    g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);
                    g1 = g.GetComponentInChildren<auxHealth>().gameObject;

                    visualHealth = g.GetComponentInChildren<auxHealth>().i;
                }
                else
                {
                    Destroy(g);
                    g = null;
                }

            }
        }
        else {
			// Only display HealthBar on GameObjects that are Unfogged
			if (unitLOSEntity.RevealState.Equals (LOSEntity.RevealStates.Hidden) ||
			    unitLOSEntity.RevealState.Equals (LOSEntity.RevealStates.Fogged)
			)
				g.SetActive(false);
			else
				g.SetActive (true);


            g1.SetActive(true);

            g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);

            maxXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x;
            minXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x - g.GetComponentInChildren<auxHealth>().gray.rectTransform.rect.width;

            curHealth = GetComponent<Health>().getHealth();
            HandleHealth();

            if (g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.y <= Screen.height * GameController.Instance.UIheight) g.GetComponentInChildren<auxHealth>().gameObject.SetActive(false);
            


                /*g1.SetActive(true);

                if (g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.y > Screen.height * GameController.Instance.UIheight)
                {

                    g.GetComponentInChildren<auxHealth>().gameObject. SetActive(true);

                    g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);

                    maxXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x;
                    minXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x - g.GetComponentInChildren<auxHealth>().gray.rectTransform.rect.width;

                    curHealth = GetComponent<Health>().getHealth();
                    HandleHealth();

                }
                else
                {
                    g.GetComponentInChildren<auxHealth>().gameObject.SetActive(false);
                }*/


            }       
    }

   

    private void HandleHealth()
    {
        
        float currentXValue = MapValues(curHealth, 0, maxHealth, minXValue, maxXValue);
        
        visualHealth.rectTransform.position -= new Vector3(visualHealth.rectTransform.position.x - currentXValue, 0, 0);


        if (curHealth > maxHealth / 2)
        {  
            visualHealth.color = new Color32((byte)MapValues(curHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            visualHealth.color = new Color32(255, (byte)MapValues(curHealth, 0, maxHealth / 2, 0, 255), 0, 255);
        }
    }



    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void destroyBar()
    {
        Destroy(g);
        g = null;
    }
}


