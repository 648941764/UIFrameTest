using System.Collections.Generic;

public partial class DataManager
{
    public Dictionary<int, TestData> testDatas { get; private set; }
    public Dictionary<int, PlayerData> playerDatas { get; private set; }
    public Dictionary<int, ItemData> itemDatas { get; private set; }
    public Dictionary<int, EnemyData> enemyDatas { get; private set; }
    public Dictionary<int, ShopData> shopDatas { get; private set; }
    public Dictionary<int, EquipmentData> equipmentDatas { get; private set; }
    public Dictionary<int, DropData> dropDatas { get; private set; }
}