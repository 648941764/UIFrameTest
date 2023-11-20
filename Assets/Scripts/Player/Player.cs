using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int level;
    public int exp;
    public int hp;
    public int maxhp;
    public int maxExp;

    private Dictionary<int, PlayerData> playerCfg => DataManager.Instance.PlayerDatas;
    public Player()
    {
        Debug.Log(playerCfg);
    }
}
