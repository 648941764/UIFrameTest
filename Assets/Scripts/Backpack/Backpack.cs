using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public int id, amount;
}
public class Backpack
{
    public const int ITEMS_NUM = 45;
    private Item[] items;
    private Dictionary<int, ItemData> itemCfg => DataManager.Instance.ItemDatas;
    
    private Backpack() 
    {
        items = new Item[ITEMS_NUM];
    }

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

    public void AddItem(Item item)
    {
        ItemData cfg = GetCfg(item.id);
        if (cfg != null)
        {
            int index = -1;
            while (++index < ITEMS_NUM)
            {
                if (items[index] == null)
                {
                    items[index] = new Item() {id = item.id, amount = item.amount };
                    break;
                }
            }
        }
    }

    public void AddItem(int id, int amount)
    {
        AddItem(new Item() { id = id, amount = amount });
    }

    public void RemoveItem(Item item)
    {
        int i = -1;
        while (++i < ITEMS_NUM)
        {
            if (items[i] == item)
            {
                items[i] = null;
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
    }
}
