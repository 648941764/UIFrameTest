using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using UnityEngine;

public sealed partial class DataManager : Singleton<DataManager>
{
    public const string DATA_PATH = "Datas/{0}";

    public void LoadDatas()
    {
        LoadTestDatas();
        LoadPlayerDatas();
        LoadItemDatas();
        LoadEnemyDatas();
        LoadDropDatas();
        LoadShopDatas();
    }
}
