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
    private PlayerData playerData => playerCfg[1];
    public Player()
    {
        level = playerData.level;
        exp = playerData.exp;
        hp = playerData.hp;
        maxhp = playerData.maxhp;
        maxExp = playerData.maxExp;
    }
}
