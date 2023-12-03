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
/// backpack�������е����ݶ���Test�ű�����
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
    //    //���ж��Ƿ��ǿ������ĵ���Ʒ������ǣ���ô��ִ���������
    //    int differ = item.amount - amount;//differ���ж��Ƿ�����
    //    if (differ <= 0)
    //    {
    //        //ִ��ʹ����Ʒ����߼�
    //        for (int i = 0; i < item.amount; i++)
    //        {
    //            //ִ����Ӧ���߼�
    //            Test.Instance.Player.hp += cfg.incraseHp;
    //            if (Test.Instance.Player.hp >= Test.Instance.Player.maxHp)
    //            {
    //                Test.Instance.Player.hp = Test.Instance.Player.maxHp;
    //                Debug.Log("��ɫѪ���Ѿ��ﵽ���ֵ");
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
    //                Debug.Log("��ɫѪ���Ѿ��ﵽ���ֵ");
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
            Debug.Log("��ɫ��Ѫ������");
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
