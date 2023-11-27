using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDatas", menuName = "Datas/ShopDatas", order = 0 )]
public class ShopDatas : ScriptableObject
{
    public List<ShopData> datas = new List<ShopData>();
}

[Serializable]
public class ShopData
{
    public int id;
    public int price;
    public ItemData item;
}
