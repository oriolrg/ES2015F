using UnityEngine;
using System.Collections;

public class ConvertToPlayersWall : MonoBehaviour
{
    public Player player;

    void Start()
    {
        GameObject correctWall = DataManager.Instance.civilizationDatas[GameData.playerToCiv(player)].units[UnitType.Wall];
        Identity myIdentity = GetComponent<Identity>();
        Identity correctIdenity = correctWall.GetComponent<Identity>();
        if (correctIdenity == null) print("null");
        print(correctIdenity.civilization + " " + myIdentity.civilization);
        
        if ( myIdentity == null || ( myIdentity.civilization != correctIdenity.civilization))
        {
            GameObject created = Instantiate(correctWall, transform.position, correctWall.transform.rotation) as GameObject;
            created.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }
    }
}
