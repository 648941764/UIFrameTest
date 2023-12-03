using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class BackpackForm : Form, IPointerClickHandler
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _slotPrefab;
    [SerializeField] private ItemUI _slotUIPrefab;
    [SerializeField] private RectTransform _dragItemParent;
    [SerializeField] private Image Image;
    [SerializeField] private Button DelBtn;

    private ItemUI[] itemUIs;
    private Item ChooseItem;
    private RectTransform[] itemSlots;
    public RectTransform[] ItemSlots => itemSlots;
    public RectTransform DragItem => _dragItemParent;
    public Image image => Image;
    public ItemUI UIPrefab => _slotUIPrefab;

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
        DelBtn.onClick.AddListener(DelItem);
    }

    public void DelItem()
    {
        Test.Instance.Backpack.RemoveItem(ChooseItem);
        RefreshSlotImage();
    }

    public void OnBackpackDataChangeHandle(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.BackpackItemChange)
        {
            OnRefresh();
        }
    }

    public int FindNearestItemSlot(Image image)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(itemSlots[i], image.transform.position))
            {
                return i;
            }
        }
        return -1;
    }

    public void RefreshSlotImage()
    {
        int k = -1;
        while (++k < Backpack.ITEM_NUMS)
        {
            itemSlots[k].GetComponent<Image>().color = Color.white;
        }
        ChooseItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Test.Instance.Backpack.SortItme();//排序
        Vector2 mouesPos = eventData.position;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(itemSlots[i], mouesPos))
            {
               Item currentItem = Test.Instance.Backpack.Items[i];

                if (currentItem == ChooseItem)
                {
                    ChooseItem = null;
                    itemSlots[i].GetComponent<Image>().color = Color.white;
                }
                else if(currentItem != null)
                {
                    RefreshSlotImage();
                    ChooseItem = currentItem;
                    itemSlots[i].GetComponent<Image>().color = Color.gray;
                }
                break;
            }
        }
    }
}
