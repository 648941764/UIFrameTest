using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

/// <summary>
/// 测试类代码
/// </summary>
public partial class DataManager
{
    /*Application.persistentDataPath + "/Players"*/
    //public void SaveDataByShopDict(Dictionary<int, ShopData> shopDataDict)
    //{
    //    string path = Path.Combine(Application.persistentDataPath, dataPath);
    //    string json = JsonConvert.SerializeObject(shopDataDict);
    //    File.WriteAllText(path, json);
    //}

    //public Dictionary<int, ShopData> LoadDataByShopDict()
    //{
    //    string path = Path.Combine(Application.persistentDataPath, dataPath);
    //    if (File.Exists(path))
    //    {
    //        string json = File.ReadAllText(path);
    //        Dictionary<int, ShopData> shopdata = JsonConvert.DeserializeObject<Dictionary<int, ShopData>>(json);
    //        return shopdata;
    //    }
    //    return null;
    //}
    //=========================================================================================================================

    public void SavePlayerJsonData(CharacterEntity playerEntity)
    {
        if(!File.Exists(Application.persistentDataPath + "/Players"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Players");
        }
        
        //string jsonData = JsonConvert.SerializeObject(playerEntity);
        PlayerEntityData data = new PlayerEntityData();
        data.health = playerEntity.GetHealth();
        data.exp = playerEntity.GetExp();
        data.maxHealth = playerEntity.GetMaxHealth();
        data.attack = playerEntity.GetAttack();
        data.defence = playerEntity.GetDefence();
        data.gold = playerEntity.GetGold();
        data.level = playerEntity.GetLevel();
        data.uid = playerEntity.UID;
        data.id = playerEntity.ID;
        string jsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.persistentDataPath + "/Players/PlayerData.json", jsonData);

    }

    public CharacterEntity LoadPlayerJsonData()
    {
        string path = Application.persistentDataPath + string.Format("/Players/PlayerData.json");
        if(File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PlayerEntityData data = JsonConvert.DeserializeObject<PlayerEntityData>(jsonData);
            CharacterEntity playerEntity = new CharacterEntity();
            playerEntity.SetID(data.id);
            playerEntity.SetHealth(data.health);
            playerEntity.SetExp(data.exp);
            playerEntity.SetUID(data.uid);
            playerEntity.SetMaxHealth(data.maxHealth);
            playerEntity.SetAttack(data.attack);
            playerEntity.SetDefence(data.defence);
            playerEntity.SetLevel(data.level);
            playerEntity.SetGold(data.gold);
            return playerEntity;
        }
        return null;
    }

    public void SaveRefreshTime()
    {
        if (!File.Exists(Application.persistentDataPath + "/Players"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Players");
        }
        DateTime refreshTime = DateTime.Now;
        refreshTime.AddDays(3);
        string jsonData = JsonConvert.SerializeObject(refreshTime);
        File.WriteAllText(Application.persistentDataPath + "/Players/ShopRefreshTime.json", jsonData);
        Debug.Log("时间保存成功");
    }

    public DateTime LoadRefreshTime()
    {
        string path = Application.persistentDataPath + string.Format("/Players/ShopRefreshTime.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            DateTime refreshTime = JsonConvert.DeserializeObject<DateTime>(jsonData);
            return refreshTime;
        }
        return DateTime.Now.AddDays(3);
    }
}

public class PlayerEntityData
{
    public int id;
    public int uid;
    public int health;
    public int maxHealth;
    public int level;
    public int exp;
    public int attack;
    public int defence;
    public int gold;
}


