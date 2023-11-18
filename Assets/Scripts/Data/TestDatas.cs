using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TestDatas", menuName = "Datas/TestDatas", order = 0)]
public class TestDatas : ScriptableObject
{
    public List<TestData> datas = new List<TestData>();
}

[Serializable]
public class TestData
{
    public int id;
    public Vector3 pos;
}

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

[CreateAssetMenu(fileName = "NPCDatas", menuName = "Datas/NPCDatas", order = 0)]
public class NPCDatas : ScriptableObject
{
    public List<NPCData> datas = new List<NPCData>();
}

[Serializable]
public class NPCData
{
    public int id;
    public int sex;
    public int hp;
}
