using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDatas", menuName = "Datas/PlayerDatas")]
public class PlayerDatas : ScriptableObject
{
    public List<PlayerData> datas = new List<PlayerData>();
}

[Serializable]
public class PlayerData
{
    public int attack;
    public int id;
    public int level;
    public int exp;
    public int hp;
    public int maxHp;
    public int maxExp;
    public int defence;
    public int gold;
}
