using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int attack;
    public int level;
    public int exp;
    public int hp;
    public int maxHp;
    public int maxExp;
    public int defence;

    private Dictionary<int, PlayerData> playerCfg => DataManager.Instance.PlayerDatas;
    private PlayerData playerData => playerCfg[1];
    public Player()
    {
        attack = playerData.attack;
        level = playerData.level;
        exp = playerData.exp;
        hp = playerData.hp;
        maxHp = playerData.maxhp;
        maxExp = playerData.maxExp;
        defence = playerData.defence;
    }

    public void ChangeHp(int change)
    {
        hp = Mathf.Clamp(hp + change, 0, maxHp);
    }
}
