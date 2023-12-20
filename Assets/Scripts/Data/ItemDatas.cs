using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatas", menuName = "Datas/ItemDatas")]
public class ItemDatas : ScriptableObject
{
    public List<ItemData> datas = new List<ItemData>();
}

[Serializable]
public class ItemData
{
    public ItemEnum itemType;
    public int id;
    public Sprite sprite;
    public string name;
    public int incraseHp;
    public int attack;
    public int defence;

}
