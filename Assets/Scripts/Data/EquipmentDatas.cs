using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EquipmentDatas", menuName = "Datas/EquipmentDatas", order = 0)]
public class EquipmentDatas : ScriptableObject
{
    public List<EquipmentData> datas = new List<EquipmentData>();
}

[Serializable]
public class EquipmentData
{
    public int id;
    public int attack;
    public int defence;
    public Sprite sprite;
}
