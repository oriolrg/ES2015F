using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    private float MaxHealth;
    [SerializeField] private float health;
    private float auxHealth;
    public float HealthRatio { get { return health / MaxHealth; } }

    public GameObject destroyedPrefab;
    public bool changedMesh = false;
    
    void Start()
    {
        UnitType myType = gameObject.GetComponentOrEnd<Identity>().unitType;
        MaxHealth = DataManager.Instance.unitDatas[myType].stats[Stat.Health];
        health = MaxHealth;
        auxHealth = health;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            loseHP(10);
        }

        if (!changedMesh && GetComponent<Identity>().unitType.isBuilding() && health <= MaxHealth / 2)
        {
            if (!GetComponent<BuildingConstruction>().getConstructionOnGoing())
            {
                //GetComponent<MeshFilter>().mesh = destroyedPrefab.GetComponent<MeshFilter>().sharedMesh;
                if (destroyedPrefab != null)
                {
                    foreach (Transform child in destroyedPrefab.transform)
                    {
                        GameObject newFirePrefab = Instantiate(child.gameObject, child.position, Quaternion.identity) as GameObject;
                        newFirePrefab.transform.SetParent(transform);
                        newFirePrefab.transform.localPosition = child.localPosition;
                        newFirePrefab.transform.localRotation = child.localRotation;

                    }
                }
                changedMesh = true;
            }
        }
    }

    public void loseHP(int hpLost)
    {
		AI.Instance.counterattack (gameObject);
        auxHealth -= hpLost;
        health -= hpLost;
        health = Mathf.Min(health, MaxHealth);
        health = Mathf.Max(health, 0);
        if (health <= 0)
        {
            die();

            healthbar h = GetComponent<healthbar>();
            if(h!=null) h.destroyBar();
        }
        GameController.Instance.updateHealth(gameObject);
    }

    public void die()
    {
        GameController.Instance.removeUnit(gameObject);

        if (GetComponent<Identity>().unitType.isBuilding())
        {
            BuildingConstruction b = GetComponent<BuildingConstruction>();
            if (b != null && b.getConstructionOnGoing())
            {
                foreach (var unit in b.getConstructingUnits()) unit.GetComponent<Construct>().SetInConstruction(false);
            }

            Destroy(gameObject);
        }
        else
        {
            Construct c = GetComponent<Construct>();
            if (c.getInConstruction() || c.getConstruct())
            {
                if(c.getBuildingToConstruct() != null) c.getBuildingToConstruct().GetComponent<BuildingConstruction>().deleteUnit(gameObject);
                c.SetInConstruction(false);
                c.setConstruct(false);
            }

            Animator ani = GetComponent<Animator>();
            if (ani != null)
            {
                ani.SetBool("die", true);
                Destroy(gameObject, 3);
            }
        }
    }

    public float getMaxHealth()
    {
        return MaxHealth;
    }

    public float getHealth()
    {
        return health;
    }

    public float getAuxHealth()
    {
        return auxHealth;
    }

    public void setHealth(float h)
    {
        health = h;
    }
    public void setMaxHealth(float h)
    {
        MaxHealth = h;
    }

    public void setAuxHealth(float h)
    {
        auxHealth = h;
    }
}