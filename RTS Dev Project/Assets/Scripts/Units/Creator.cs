using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject prefab;
    public Sprite sprite;
    public float time = 5;
    private Creation villager;

    // Use this for initialization
    void Start ()
    {
        villager = new Creation(time,sprite,addVillager);
	}


    public void OnMouseDown()
    {
        GameController.Instance.addCreation(villager);
    }

    public void addVillager()
    {
        Instantiate(prefab, transform.position + 3 * -transform.up, Quaternion.identity);
    }


	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
