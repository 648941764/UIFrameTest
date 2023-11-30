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
    public ItemEnum itemType;
    public Sprite sprite;
    public string name;
    public int incraseHp;
    public float attack;
    public float defence;
}
