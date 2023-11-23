using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyrDatas", menuName = "Datas/EnemyDatas", order = 0)]
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
    public int deathGold;
    public int deathExp;

}
