using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PlayerDatas", menuName = "Datas/PlayerDatas", order = 0)]
public class PlayerDatas : ScriptableObject
{
    public List<PlayerData> datas = new List<PlayerData>();
}

[Serializable]
public class PlayerData
{
    public int id;
    public int sex;
    public int hp;
}
