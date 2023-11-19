using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class BackpackForm : Form
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _slotPrefab;
    [SerializeField] private RectTransform _slotUIPrefab;

    private Dictionary<RectTransform, Image> itmeSprite = new Dictionary<RectTransform, Image>();
    private Dictionary<RectTransform, Text> text = new Dictionary<RectTransform, Text>();

    private RectTransform[] itemUIs;
    private RectTransform[] itemSlots;
    Dictionary<int, ItemData> dict_itmedatas = new Dictionary<int, ItemData>();
    private const int ITEM_NUMS = 45;
    
    
    protected override void OnRefresh()
    {
        base.OnRefresh();
    }

    protected override void InitAwake()
    {
        dict_itmedatas = DataManager.Instance.ItemDatas;
        itemUIs = new RectTransform[ITEM_NUMS];
        itemSlots = new RectTransform[ITEM_NUMS];
        base.InitAwake();
        int index = -1;
        while (++index < ITEM_NUMS)
        {
            RectTransform slot = Instantiate(_slotPrefab, _parent);
            slot.name = _slotPrefab.name;
            itemSlots[index] = slot;
        }
    }

    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AddEvent(CreatItemUI);
    }

    public void CreatItemUI(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.BackPackAddItem)
        {
            int index = -1;
            while (++index < ITEM_NUMS)
            {
                if (itemUIs[index] == null)
                {
                    RectTransform slotui = Instantiate(_slotUIPrefab, itemSlots[index]);
                    ItemData data = dict_itmedatas[5];
                    slotui.GetComponent<Image>().sprite = data.sprite;
                    itemUIs[index] = slotui;
                    break;
                }
            }
        }
    }
}
