using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class Item
{
    public int id, amount;
}
/// <summary>
/// backpack里面所有的数据都是Test脚本里面
/// </summary>
public class Backpack
{
    public const int ITEM_NUMS = 45;
    private Item[] items = new Item[ITEM_NUMS];
    private Dictionary<int, ItemData> itemCfg => DataManager.Instance.itemDatas;

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

    //public void UseItem(Item item, int amount)
    //{
    //    ItemData cfg = GetCfg(item.id);
    //    //先判断是否是可以消耗的物品，如果是，那么久执行下面代码
    //    int differ = item.amount - amount;//differ是判断是否用完
    //    if (differ <= 0)
    //    {
    //        //执行使用物品后的逻辑
    //        for (int i = 0; i < item.amount; i++)
    //        {
    //            //执行相应的逻辑
    //            Test.Instance.Player.hp += cfg.incraseHp;
    //            if (Test.Instance.Player.hp >= Test.Instance.Player.maxHp)
    //            {
    //                Test.Instance.Player.hp = Test.Instance.Player.maxHp;
    //                Debug.Log("角色血量已经达到最大值");
    //                break;
    //            }
    //        }
    //        RemoveItem(item);
    //    }
    //    else
    //    {
    //        for(int i = 0; i < amount; i++)
    //        {
    //            Test.Instance.Player.hp += cfg.incraseHp;
    //            if (Test.Instance.Player.hp >= Test.Instance.Player.maxHp)
    //            {
    //                Test.Instance.Player.hp = Test.Instance.Player.maxHp;
    //                Debug.Log("角色血量已经达到最大值");
    //                break;
    //            }
    //            item.amount--;  
    //        }
    //    }
    //    EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
    //}

    public void UseItem(Item item, int amount)
    {
        ItemData itemcfg = GetCfg(item.id);
        int useAmount = Mathf.Min(item.amount, amount);
        int actualIncreaseHp = Mathf.Min(useAmount * itemcfg.incraseHp, Test.Instance.Player.maxHp - Test.Instance.Player.hp);
        Test.Instance.Player.hp += actualIncreaseHp;

        if (Test.Instance.Player.hp >= Test.Instance.Player.maxHp)
        {
            Test.Instance.Player.maxHp = Test.Instance.Player.hp;
            Debug.Log("角色的血量已满");
        }

        if (item.amount <= useAmount)
        {
            RemoveItem(item);
        }
        else
        {
            item.amount -= useAmount;
        }
        EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
    }
}
