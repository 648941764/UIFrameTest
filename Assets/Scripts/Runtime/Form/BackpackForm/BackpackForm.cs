using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class BackpackForm : Form
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _slotPrefab;
    [SerializeField] private ItemUI _slotUIPrefab;
    [SerializeField] private RectTransform _dragItem;
    [SerializeField] private RectTransform _panel;

    private ItemUI[] itemUIs;
    private RectTransform[] itemSlots;
    public RectTransform[] ItemSlots => itemSlots;

    protected override void InitComponents()
    {
        itemUIs = new ItemUI[Backpack.ITEM_NUMS];
        itemSlots = new RectTransform[Backpack.ITEM_NUMS];
        int index = -1;
        while (++index < Backpack.ITEM_NUMS)
        {
            RectTransform slot = Instantiate(_slotPrefab, _parent);
            slot.name = _slotPrefab.name;
            itemSlots[index] = slot;
        }
    }

    protected override void OnRefresh()
    {
        base.OnRefresh();

        int i = -1;
        //获取最开始Test里面的backpack数据
        Item[] items = Test.Instance.Backpack.Items;
        while (++i < Backpack.ITEM_NUMS)
        {
            if (items[i] == null)
            {
                if (itemUIs[i] != null)
                {
                    itemUIs[i].SetActivate(false);
                }
                continue;
            }

            if (itemUIs[i] == null)
            {
                itemUIs[i] = Instantiate<ItemUI>(_slotUIPrefab, itemSlots[i]);
                itemUIs[i].transform.localPosition = Vector3.zero;
                itemUIs[i].Setindex(i);
            }
            else
            {
                itemUIs[i].SetActivate(true);
            }
            itemUIs[i].RefreshItemUI(items[i]);
        }
    }

    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AddEvent(OnBackpackDataChangeHandle);
    }

    public void OnBackpackDataChangeHandle(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.BackpackItemChange)
        {
            OnRefresh();
        }
    }

    public void SetItemUIToDragLayer(ItemUI itemUI)
    {
        itemUI.transform.SetParent(_dragItem);
    }

    public int FindNearestItemSlot(Vector2 position)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            Rect rect = itemSlots[i].rect;
            rect.position = itemSlots[i].position;
            if (rect.Contains(position))
            {
                return i;
            }
        }
        return -1;
    }

}
