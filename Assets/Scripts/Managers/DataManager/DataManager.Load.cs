using System.Collections.Generic;
using UnityEngine;

public partial class DataManager
{
    public void LoadTestDatas()
    {
        testDatas = new Dictionary<int, TestData>();
        TestDatas config = Resources.Load<TestDatas>(string.Format(DATA_PATH, typeof(TestDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            testDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

    public void LoadPlayerDatas()//获取PlayerData的数据
    {
        playerDatas = new Dictionary<int, PlayerData>();
        PlayerDatas config = Resources.Load<PlayerDatas>(string.Format(DATA_PATH, typeof(PlayerDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            playerDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }
    public void LoadItemDatas()
    {
        itemDatas = new Dictionary<int, ItemData>();
        ItemDatas config = Resources.Load<ItemDatas>(string.Format(DATA_PATH, typeof(ItemDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            itemDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

    public void LoadEnemyDatas()
    {
        enemyDatas = new Dictionary<int, EnemyData>();
        EnemyDatas config = Resources.Load<EnemyDatas>(string.Format(DATA_PATH, typeof(EnemyDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            enemyDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

    public void LoadShopDatas()
    {
        shopDatas = new Dictionary<int, ShopData>();
        ShopDatas config = Resources.Load<ShopDatas>(string.Format(DATA_PATH, typeof(ShopDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            shopDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

    public void LoadEquipmentDatas()
    {
        equipmentDatas = new Dictionary<int, EquipmentData>();
        EquipmentDatas config = Resources.Load<EquipmentDatas>(string.Format(DATA_PATH, typeof(EquipmentDatas).Name));
        int i = -1;
        while (++i < config.datas.Count)
        {
            equipmentDatas.Add(config.datas[i].id, config.datas[i]);
        }
    }

}