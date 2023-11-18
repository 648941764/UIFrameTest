using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatas", menuName = "Datas/ItemDatas", order = 0)]
public class ItemDatas : ScriptableObject
{
    public List<ItemData> datas = new List<ItemData>();
}
[Serializable]
public class ItemData
{
    public int id;
    public Sprite sprite;
    public string name;
}
