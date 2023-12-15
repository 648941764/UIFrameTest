using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DropDatas", menuName = "Datas/DropData")]
public class DropDatas : ScriptableObject
{
    public List<DropData> datas = new List<DropData>();

    public static DropItem CreateDropItem()
    {
        return Instantiate(Resources.Load<DropItem>("Prefab/DropItem/DropItem"));
    }
}

[Serializable]
public class DropData
{
    public int id;
    public List<int> items = new List<int>();
    public List<int> amounts = new List<int>();
}
