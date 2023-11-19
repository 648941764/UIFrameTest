using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public int id, amount;
}
public class Backpack
{
    public const int ITEM_NUMS = 45;
    private Item[] items = new Item[ITEM_NUMS];
    private Dictionary<int, ItemData> itemCfg => DataManager.Instance.ItemDatas;

    public Item[] Items => items;

    public Item GetItem(int id)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i] != null && items[i].id == id)
            {
                return items[i];
            }
        }

        return null;
    }

    public void AddItem(int id, int amount)
    {
        ItemData cfg = GetCfg(id);
        if (cfg != null)
        {
            int index = -1;
            while (++index < ITEM_NUMS)
            {
                if (items[index] == null)
                {
                    items[index] = new Item() { id = id, amount = amount };
                    EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
                    break;
                }
            }
        }
    }

    public void RemoveItem(Item item)
    {
        int i = -1;
        while (++i < ITEM_NUMS)
        {
            if (items[i] == item)
            {
                items[i] = null;
                EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
                break;
            }
        }
    }

    public ItemData GetCfg(int id)
    {
        if (itemCfg.TryGetValue(id, out ItemData cfg))
        {
            return cfg;
        }
        return null;
    }

    public void SwapItem(int origin, int target)
    {
        Item item = items[target];
        items[target] = items[origin];
        items[origin] = item;
        EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
    }
}
