using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {

    [SerializeField]
    private GameObject targetPrefab;
    private Identity identity;

    public GameObject attacking_target;
    public GameObject attacking_enemy;

    public double range;
    public double atkDmg;

    private Vector3 enemy_last_pos;
    private UnitMovement um;

    Animator animator;

    // Use this for initialization


    // Use this for initialization
    void Start()
    {
        this.identity = this.gameObject.GetComponent<Identity>();
        this.range = DataManager.Instance.unitDatas[identity.unitType].stats[Stat.Range];
        this.atkDmg = DataManager.Instance.unitDatas[identity.unitType].stats[Stat.Attack];
        um = gameObject.GetComponent<UnitMovement>();
        animator = GetComponent<Animator>();
    }

    public void attack(GameObject enemy)
    {
        Debug.Log(enemy.gameObject.layer);
        double r;
        if (enemy.gameObject.layer == 11)
        {

            r = this.range + 7;
        }
        else
        {
            r = this.range;
        }
        this.attacking_enemy = enemy;

        Vector3 myPos = this.gameObject.transform.position;
        Vector3 enemyPos = enemy.gameObject.transform.position;

        double d = Vector3.Distance(myPos, enemyPos);

        if (d >= range)
        {
            um.status = Status.running;
            Vector3 vec = enemyPos - myPos;
            vec = vec.normalized;
            double alpha = d - (r / 2.0);

            vec.x *= (float)alpha;
            vec.z *= (float)alpha;
            Vector3 targetPos = myPos + vec;
            targetPos.y = 0;

            attacking_target = Instantiate(targetPrefab, targetPos, Quaternion.identity) as GameObject; //Instanciar prefab target
            um.startMoving(attacking_target);
            this.enemy_last_pos = attacking_enemy.transform.position;
        }
        else
        {
            um.status = Status.attacking;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this.attacking_enemy != null)
        { //check if position has changed, and follow if so
            Vector3 enemyPos = attacking_enemy.transform.position;

            if (this.attacking_enemy.layer == 11 && (Vector3.Distance(enemyPos, this.gameObject.transform.position) <= range * 2 + 7))
            {
                attacking_enemy.GetComponent<Health>().loseHP((int)this.atkDmg);
            }
            else if (Vector3.Distance(enemyPos, this.gameObject.transform.position) <= this.range * 2)
            {
                attacking_enemy.GetComponent<Health>().loseHP((int)this.atkDmg);
            }

            if (Vector3.Distance(enemy_last_pos, enemyPos) > this.range)
            {
                this.attack(attacking_enemy);
            }
        }
    }
}

