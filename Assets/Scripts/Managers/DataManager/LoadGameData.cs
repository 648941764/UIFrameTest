using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


/// <summary>
/// ≤‚ ‘¿‡¥˙¬Î
/// </summary>
public partial class DataManager
{
    private string dataPath = "shopDataDict.json";
    public void SaveDataByShopDict(Dictionary<int, ShopData> shopDataDict)
    {
        string path = Path.Combine(Application.persistentDataPath, dataPath);
        string json = JsonConvert.SerializeObject(shopDataDict);
        File.WriteAllText(path, json);
    }

    public Dictionary<int, ShopData> LoadDataByShopDict()
    {
        string path = Path.Combine(Application.persistentDataPath, dataPath);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Dictionary<int, ShopData> shopdata = JsonConvert.DeserializeObject<Dictionary<int, ShopData>>(json);
            return shopdata;
        }
        return null;
    }
    //=========================================================================================================================
}