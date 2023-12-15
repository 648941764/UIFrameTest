using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Datas/EnemyDatas")]
public class EnemyDatas : ScriptableObject
{
    public List<EnemyData> datas = new List<EnemyData>();
}

[Serializable]
public class EnemyData
{
    public int id;
    public int hp;
    public int defence;
    public int attack;
    public int Gold;
    public int Exp;
    public int dropId;
}
