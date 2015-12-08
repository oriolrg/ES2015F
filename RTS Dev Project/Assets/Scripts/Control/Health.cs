using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    private float MaxHealth;
    [SerializeField] private float health;
    public float HealthRatio { get { return health / MaxHealth; } }

    public GameObject destroyedPrefab;
    public bool changedMesh = false;
    
    void Start()
    {
        UnitType myType = gameObject.GetComponentOrEnd<Identity>().unitType;
        MaxHealth = DataManager.Instance.unitDatas[myType].stats[Stat.Health];
        health = MaxHealth;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            loseHP(10);
        }

        if (!changedMesh && GetComponent<Identity>().unitType.isBuilding() && health <= MaxHealth / 2)
        {   
            //GetComponent<MeshFilter>().mesh = destroyedPrefab.GetComponent<MeshFilter>().sharedMesh;
            foreach(Transform child in destroyedPrefab.transform)
            {
                GameObject newFirePrefab = Instantiate(child.gameObject, child.position, Quaternion.identity) as GameObject;
                newFirePrefab.transform.SetParent(transform);
                newFirePrefab.transform.localPosition = child.localPosition;
                newFirePrefab.transform.localRotation = child.localRotation;

            }
            changedMesh = true;
        }
    }

    public void loseHP(int hpLost)
    {
		AI.Instance.counterattack (gameObject);
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
            if (GetComponent<Construct>().getInConstruction() || GetComponent<Construct>().getConstruct())
            {
                GetComponent<Construct>().getBuildingToConstruct().GetComponent<BuildingConstruction>().deleteUnit(gameObject);
                GetComponent<Construct>().SetInConstruction(false);
                GetComponent<Construct>().setConstruct(false);
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
}