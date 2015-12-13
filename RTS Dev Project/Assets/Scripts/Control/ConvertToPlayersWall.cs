using UnityEngine;
using System.Collections;

public class ConvertToPlayersWall : MonoBehaviour
{
    public Player player;

    void Start()
    {
        print(GameData.playerToCiv(player));
        GameObject correctWall = DataManager.Instance.civilizationDatas[GameData.playerToCiv(player)].units[UnitType.Wall];
        GameObject created = Instantiate(correctWall, transform.position, correctWall.transform.rotation) as GameObject;
        created.transform.SetParent(transform.parent);
        Destroy(gameObject);
    }
}
