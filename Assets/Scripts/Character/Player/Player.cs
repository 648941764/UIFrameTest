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
    public int gold;

    private Dictionary<int, PlayerData> playerCfg => DataManager.Instance.playerDatas;
    private PlayerData playerData => playerCfg[4001];
    public Player()
    {
        attack = playerData.attack;
        level = playerData.level;
        exp = playerData.exp;
        hp = playerData.hp;
        maxHp = playerData.maxhp;
        maxExp = playerData.maxExp;
        defence = playerData.defence;
        gold = playerData.gold;
    }

    public void ChangeHp(int change)
    {
        hp = Mathf.Clamp(hp + change, 0, maxHp);
    }
}
