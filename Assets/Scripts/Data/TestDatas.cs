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
