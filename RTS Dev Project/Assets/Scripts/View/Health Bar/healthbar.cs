using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{

    public float maxHealth;
    public float curHealth;
    public float auxMaxHealth;

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

        if (GetComponent<Identity>().unitType.isBuilding())
        {
            //GetComponent<Health>().setHealth(50);
            //GetComponent<Health>().setMaxHealth(50);
            //GetComponent<Health>().setAuxHealth(50);
            auxMaxHealth = GetComponent<BuildingConstruction>().timer;
            maxHealth = MapValues(auxMaxHealth, 0, auxMaxHealth, GetComponent<Health>().getMaxHealth() / 10, GetComponent<Health>().getMaxHealth());
            curHealth = MapValues(auxMaxHealth - GetComponent<BuildingConstruction>().timer, 0, auxMaxHealth, GetComponent<Health>().getMaxHealth() / 10, GetComponent<Health>().getMaxHealth());

        }
        else
        {
            maxHealth = GetComponent<Health>().getMaxHealth();
            curHealth = GetComponent<Health>().getHealth();
        }

        g = Instantiate(prefab);
        g.transform.SetParent(GameController.Instance.healthBarsParent.transform);

        g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);
        g1 = g.GetComponentInChildren<auxHealth>().gameObject;

        visualHealth = g.GetComponentInChildren<auxHealth>().i;

        unitLOSEntity = gameObject.GetComponent<LOSEntity>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (g != null)
        {

            g1.SetActive(true);

            g.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(GetComponentInChildren<auxHealth>().gameObject.transform.position);
            //auxHealth aH = g.GetComponentInChildren<auxHealth>();

            maxXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x;
            minXValue = g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.x - g.GetComponentInChildren<auxHealth>().gray.rectTransform.rect.width;


            if (GetComponent<Identity>().unitType.isBuilding())
            {
                //if (GetComponent<BuildingConstruction>().getPhase() == 2)
                if (GetComponent<BuildingConstruction>().getConstructionOnGoing())
                {
                    //maxHealth = auxMaxHealth;
                    //curHealth = maxHealth - GetComponent<BuildingConstruction>().timer;
                    float dif = GetComponent<Health>().getMaxHealth() - GetComponent<Health>().getAuxHealth();
                    maxHealth = MapValues(auxMaxHealth, 0, auxMaxHealth, GetComponent<Health>().getMaxHealth() / 10, GetComponent<Health>().getMaxHealth());
                    curHealth = MapValues(auxMaxHealth - GetComponent<BuildingConstruction>().timer, 0, auxMaxHealth, GetComponent<Health>().getMaxHealth() / 10, GetComponent<Health>().getMaxHealth()) - dif;
                    GetComponent<Health>().setHealth(curHealth);
                }
                else
                {
                    maxHealth = GetComponent<Health>().getMaxHealth();
                    curHealth = GetComponent<Health>().getHealth();
                }
            }
            else
            {
                curHealth = GetComponent<Health>().getHealth();
            }

            if (g != null)
            {

                HandleHealth();

            if (unitLOSEntity != null)
            {
                if (!GameObject.FindGameObjectWithTag("Ground").GetComponent<LOSManager>().revealed)
                {
                    if (g.transform.GetChild(0).transform.position.z < 0 || g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.y <= Screen.height * GameController.Instance.UIheight || unitLOSEntity.RevealState.Equals(LOSEntity.RevealStates.Hidden) ||
                    unitLOSEntity.RevealState.Equals(LOSEntity.RevealStates.Fogged))
                    {
                        g.GetComponentInChildren<auxHealth>().gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (g.transform.GetChild(0).transform.position.z < 0 || g.GetComponentInChildren<auxHealth>().gray.rectTransform.position.y <= Screen.height * GameController.Instance.UIheight)
                        g.GetComponentInChildren<auxHealth>().gameObject.SetActive(false);
                }

            }            


               

                }



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



