using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    private float MaxHealth;
    [SerializeField] private float health;
    public float HealthRatio { get { return health / MaxHealth; } }
    
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
    }

    public void loseHP(int hpLost)
    {
        health -= hpLost;
        health = Mathf.Min(health, MaxHealth);
        health = Mathf.Max(health, 0);
        if (health <= 0)
        {
            die();
        }
        GameController.Instance.updateHealth(gameObject);
    }

    public void die()
    {
        GameController.Instance.removeUnit(gameObject);

        if (GetComponent<Identity>().unitType.isBuilding())
            Destroy(gameObject);
        else
        {

            Animator ani = GetComponent<Animator>();
            if (ani != null)
            {
                ani.SetBool("die", true);
                Destroy(gameObject, 3);
            }
        }
    }
}