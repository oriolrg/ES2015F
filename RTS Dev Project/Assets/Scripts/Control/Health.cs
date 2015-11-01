using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int MaxHealth;
    private int health;
    public float HealthRatio { get { return (float)health / MaxHealth; } }
	// Use this for initialization
	void Start () {
        health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Z)) loseHP(10);
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
    }

    private void die()
    {
        GameController.Instance.removeUnit(gameObject);
        Animator ani = GetComponent<Animator>();
        if (ani != null) ani.SetBool("dead", true);
        Destroy(gameObject, 3);
    }
}
