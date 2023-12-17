using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameItem
{
    public int amount, id;
}

public class GameBackpack
{
    public const int ITEM_NUM = 30;
    private GameItem[] items = new GameItem[ITEM_NUM];

    public GameItem[] Items => items;

    public Dictionary<int, ItemData> itemdata => DataManager.Instance.itemDatas;


    public void Additem(int id, int amount)
    {
        ItemData data = GetData(id);
        if (data != null)
        {
            int i = -1;
            while (++i < ITEM_NUM)
            {
                if (items[i]  == null)
                {
                    items[i] = new GameItem() { amount = amount, id = id };
                    EventManager.Instance.Broadcast(EventParam.Get(EventType.BackpackItemChange));
                    break;
                }
            }
        }

    }

    public void RemoveItme(GameItem item)
    {
        int i = -1;
        while (++i < ITEM_NUM)
        {
            if (items[i] == item)
            {
                items[i] = null;
                EventManager.Instance.Broadcast(EventParam.Get(EventType.BackpackItemChange));
            }
        }
    }

    public ItemData GetData(int id)
    {
        if (itemdata.TryGetValue(id, out ItemData data))
        {
            return data;
        }
        return null;
    }

    public void SwapItem(int origin, int target)
    {
        GameItem item = items[target];
        items[target] = items[origin];
        items[origin] = item;
        EventManager.Instance.Broadcast(EventParam.Get(EventType.BackpackItemChange));
    }
}
