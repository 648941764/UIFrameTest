using System.Collections.Generic;
using UnityEngine;

public partial class DataManager
{
    public void LoadTestDatas()
    {
        TestDatas = new Dictionary<int, TestData>();
        TestDatas config = Resources.Load<TestDatas>(string.Format(DATA_PATH, typeof(TestDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            TestDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

    public void LoadPlayerDatas()//获取PlayerData的数据
    {
        PlayerDatas = new Dictionary<int, PlayerData>();
        PlayerDatas config = Resources.Load<PlayerDatas>(string.Format(DATA_PATH, typeof(PlayerDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            PlayerDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }
    public void LoadItemDatas()
    {
        ItemDatas = new Dictionary<int, ItemData>();
        ItemDatas config = Resources.Load<ItemDatas>(string.Format(DATA_PATH, typeof(ItemDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            ItemDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

}